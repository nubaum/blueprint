using Blueprint.Abstractions.Adapters.Application.Workspace;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Application.Workspace.Models;
using Blueprint.Abstractions.Infrastructure;

namespace Blueprint.Application.Handlers.Workspace;

internal sealed class FolderTreeBuilder(
    IDirectoryInfoProvider directoryInfoProvider,
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

        var directoryInfo = new DirectoryInfo(rootFolderPath);

        return dtoAdapter.ToProjectTreeNode(BuildDirectoryNode(directoryInfo));
    }

    private FolderTreeDto BuildDirectoryNode(DirectoryInfo directory)
    {
        var node = new FolderTreeDto
        {
            Name = directory.Name,
            FullPath = directory.FullName,
            IsFolder = true
        };

        foreach (DirectoryInfo? childDirectory in directoryInfoProvider.GetDirectoriesSafely(directory).OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase))
        {
            node.Children.Add(BuildDirectoryNode(childDirectory));
        }

        foreach (FileInfo? childFile in directoryInfoProvider.GetFilesSafely(directory).OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase))
        {
            node.Children.Add(new FolderTreeDto
            {
                Name = childFile.Name,
                FullPath = childFile.FullName,
                IsFolder = false
            });
        }

        return node;
    }
}
