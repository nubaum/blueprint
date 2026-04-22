using System.Collections.Concurrent;
using System.IO.Enumeration;
using System.Threading.Channels;
using Blueprint.Application.Abstractions.Infrastructure;
using Blueprint.Application.Abstractions.Infrastructure.Models;

namespace Blueprint.Infrastructure;

internal class DirectoryScanner : IDirectoryScanner
{
    private static readonly EnumerationOptions _enumerationOptions = new()
    {
        IgnoreInaccessible = true,
        RecurseSubdirectories = false,
        AttributesToSkip = FileAttributes.Hidden | FileAttributes.System
    };

    public async Task<FileSystemItem> ScanAsync(
        FileSystemScanOptions options,
        CancellationToken cancellationToken = default)
    {
        ValidatePathExists(options);

        int parallelism = GetParallelism(options);

        var flatMap = new ConcurrentDictionary<string, ConcurrentBag<(string FullPath, string Name, bool IsDirectory)>>(
            StringComparer.OrdinalIgnoreCase);

        var channel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions
        {
            SingleReader = false,
            SingleWriter = false
        });

        flatMap.TryAdd(options.RootPath, []);
        channel.Writer.TryWrite(options.RootPath);

        int pending = 1;

        async Task WorkerAsync()
        {
            await foreach (string currentDir in channel.Reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    ConcurrentBag<(string FullPath, string Name, bool IsDirectory)> parentSlot = flatMap.GetOrAdd(currentDir, _ => []);

                    var entries = new FileSystemEnumerable<(string FullPath, string Name, bool IsDirectory)>(
                        currentDir,
                        (ref e) =>
                        {
                            string fullPath = e.ToFullPath();
                            string name = e.FileName.ToString();
                            bool isDirectory = e.IsDirectory;
                            return (fullPath, name, isDirectory);
                        },
                        _enumerationOptions);

                    foreach ((string? fullPath, string? name, bool isDirectory) in entries)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        parentSlot.Add((fullPath, name, isDirectory));

                        if (isDirectory)
                        {
                            flatMap.TryAdd(fullPath, []);
                            Interlocked.Increment(ref pending);
                            channel.Writer.TryWrite(fullPath);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Log and skip directories/files that cannot be accessed
                }
                catch (DirectoryNotFoundException)
                {
                    // Log and skip directories that were deleted during scanning
                }
                catch (IOException)
                {
                    // Log and skip directories/files that caused I/O errors
                }
                finally
                {
                    if (Interlocked.Decrement(ref pending) == 0)
                    {
                        channel.Writer.Complete();
                    }
                }
            }
        }

        Task[] workers = Enumerable
            .Range(0, parallelism)
            .Select(_ => Task.Run(WorkerAsync, cancellationToken))
            .ToArray();

        await Task.WhenAll(workers);

        return BuildNode(options.RootPath, options.RootPath, FileSystemItemKind.Directory, flatMap);
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
        string name,
        FileSystemItemKind kind,
        ConcurrentDictionary<string, ConcurrentBag<(string FullPath, string Name, bool IsDirectory)>> flatMap)
    {
        if (kind == FileSystemItemKind.File || !flatMap.TryGetValue(fullPath, out ConcurrentBag<(string FullPath, string Name, bool IsDirectory)>? rawChildren))
        {
            return new FileSystemItem
            {
                Name = name,
                Kind = kind,
                SubItems = Array.Empty<FileSystemItem>()
            };
        }

        FileSystemItem[] children = [.. rawChildren
            .ToArray()
            .OrderBy(e => e.IsDirectory ? 0 : 1)
            .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
            .Select(e => BuildNode(
                e.FullPath,
                e.Name,
                e.IsDirectory ? FileSystemItemKind.Directory : FileSystemItemKind.File,
                flatMap))];

        return new FileSystemItem
        {
            Name = name,
            Kind = kind,
            SubItems = children
        };
    }
}
