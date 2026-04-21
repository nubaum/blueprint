using Blueprint.Application.Abstractions;
using Blueprint.Application.Abstractions.Infrastructure.Models;
using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.Stores;

namespace Blueprint.Application.Adapters;

internal class FolderTreeDtoAdapter : IFolderTreeDtoAdapter
{
    public IProjectTreeNode ToProjectTreeNode(FileSystemItem fileSystemItem)
    {
        var node = new ProjectTreeNode
        {
            Name = fileSystemItem.Name,
            FullPath = fileSystemItem.FullPath,
            IsFolder = fileSystemItem.Kind == FileSystemItemKind.Directory,
            Extension = fileSystemItem.Extension
        };

        foreach (FileSystemItem childItem in fileSystemItem.SubItems)
        {
            IProjectTreeNode childNode = ToProjectTreeNode(childItem);
            node.AddChild(childNode);
        }

        return node;
    }
}
