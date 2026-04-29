using System.Collections.Concurrent;
using System.IO.Enumeration;
using Blueprint.Application.Abstractions.Infrastructure;
using Blueprint.Application.Abstractions.Infrastructure.Models;

namespace Blueprint.Infrastructure.IOManagement;

public class DirectoryScanner : IDirectoryScanner
{
    private static readonly EnumerationOptions _enumerationOptions = new()
    {
        IgnoreInaccessible = true,
        RecurseSubdirectories = false,
        AttributesToSkip = FileAttributes.None
    };

    public FileSystemItem Scan(
        FileSystemScanOptions options,
        CancellationToken cancellationToken = default)
    {
        ValidatePathExists(options);

        int parallelism = GetParallelism(options);

        var flatMap = new ConcurrentDictionary<string, ConcurrentBag<RawEntry>>(
            StringComparer.OrdinalIgnoreCase);

        var directoryQueue = new ConcurrentQueue<string>();
        directoryQueue.Enqueue(options.RootPath);

        flatMap.TryAdd(options.RootPath, []);

        int activeWorkers = 0;
        using var allDone = new ManualResetEventSlim(false);

        void WorkerLoop()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!directoryQueue.TryDequeue(out string? currentDir))
                {
                    if (Interlocked.CompareExchange(ref activeWorkers, 0, 0) == 0)
                    {
                        allDone.Set();
                        return;
                    }

                    Thread.SpinWait(50);
                    continue;
                }

                Interlocked.Increment(ref activeWorkers);

                try
                {
                    ConcurrentBag<RawEntry> parentSlot = flatMap.GetOrAdd(currentDir, _ => []);

                    var entries = new FileSystemEnumerable<RawEntry>(
                        currentDir,
                        static (ref e) => new RawEntry(e.ToFullPath(), e.IsDirectory),
                        _enumerationOptions)
                    {
                        ShouldIncludePredicate = static (ref e) =>
                            !e.Attributes.HasFlag(FileAttributes.ReparsePoint)
                    };

                    foreach (RawEntry raw in entries)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        parentSlot.Add(raw);

                        if (raw.IsDirectory)
                        {
                            flatMap.TryAdd(raw.FullPath, []);
                            directoryQueue.Enqueue(raw.FullPath);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Skip directories/files we don't have access to.
                }
                catch (DirectoryNotFoundException)
                {
                    // Skip if the directory was deleted during scanning.
                }
                catch (IOException)
                {
                    // Skip on IO errors (e.g. network issues for UNC paths).
                }
                finally
                {
                    Interlocked.Decrement(ref activeWorkers);
                }
            }

            allDone.Set();
        }

        var threads = new Thread[parallelism];
        for (int i = 0; i < parallelism; i++)
        {
            threads[i] = new Thread(WorkerLoop)
            {
                IsBackground = true,
                Name = $"DirScanner-{i}"
            };
            threads[i].Start();
        }

        allDone.Wait(cancellationToken);

        return BuildNode(options.RootPath, FileSystemItemKind.Directory, flatMap);
    }

    private static int GetParallelism(FileSystemScanOptions options) => options.DegreeOfParallelism > 0
        ? options.DegreeOfParallelism
        : Environment.ProcessorCount;

    private static void ValidatePathExists(FileSystemScanOptions options)
    {
        if (!Directory.Exists(options.RootPath))
        {
            throw new DirectoryNotFoundException($"Root path not found: {options.RootPath}");
        }
    }

    private static FileSystemItem BuildNode(
        string fullPath,
        FileSystemItemKind kind,
        ConcurrentDictionary<string, ConcurrentBag<RawEntry>> flatMap)
    {
        string name = Path.GetFileName(fullPath);

        if (string.IsNullOrEmpty(name))
        {
            name = fullPath;
        }

        if (kind == FileSystemItemKind.File || !flatMap.TryGetValue(fullPath, out ConcurrentBag<RawEntry>? rawChildren))
        {
            return new FileSystemItem
            {
                Name = name,
                Kind = kind,
                FullPath = fullPath,
                SubItems = Array.Empty<FileSystemItem>()
            };
        }

        FileSystemItem[] children = [.. rawChildren
            .ToArray()
            .OrderBy(e => e.IsDirectory ? 0 : 1)
            .ThenBy(e => e.FullPath, StringComparer.OrdinalIgnoreCase)
            .Select(e => BuildNode(
                e.FullPath,
                e.IsDirectory ? FileSystemItemKind.Directory : FileSystemItemKind.File,
                flatMap))];

        return new FileSystemItem
        {
            Name = name,
            Kind = kind,
            FullPath = fullPath,
            SubItems = children
        };
    }

    private readonly record struct RawEntry(string FullPath, bool IsDirectory);
}
