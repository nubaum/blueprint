using Blueprint.Application.Abstractions;
using Blueprint.Application.Abstractions.Infrastructure;
using Blueprint.Application.Abstractions.Infrastructure.Models;
using Blueprint.Application.Abstractions.Workspace;

namespace Blueprint.Application.Services;

internal sealed class FolderTreeBuilder(
    IDirectoryScanner directoryScanner,
    IFolderTreeDtoAdapter dtoAdapter)
{
    public IProjectTreeNode Build(string rootFolderPath)
    {
        if (string.IsNullOrWhiteSpace(rootFolderPath))
        {
            throw new ArgumentException("Root folder path cannot be null or empty.", nameof(rootFolderPath));
        }

        if (!Directory.Exists(rootFolderPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {rootFolderPath}");
        }

        var scanOptions = new FileSystemScanOptions
        {
            RootPath = rootFolderPath,
            DegreeOfParallelism = 0
        };

        return dtoAdapter.ToProjectTreeNode(directoryScanner.Scan(scanOptions));
    }
}
