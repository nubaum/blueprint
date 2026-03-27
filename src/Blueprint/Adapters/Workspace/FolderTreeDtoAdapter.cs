using Blueprint.Abstractions.Adapters.Application.Workspace;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Application.Workspace.Models;
using Blueprint.Stores;

namespace Blueprint.Adapters.Workspace;

internal class FolderTreeDtoAdapter : IFolderTreeDtoAdapter
{
    public IProjectTreeNode ToProjectTreeNode(FolderTreeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var node = new ProjectTreeNode
        {
            Name = dto.Name,
            FullPath = dto.FullPath,
            IsFolder = dto.IsFolder,
            Extension = dto.Extension
        };

        foreach (FolderTreeDto childDto in dto.Children)
        {
            IProjectTreeNode childNode = ToProjectTreeNode(childDto);
            node.AddChild(childNode);
        }

        return node;
    }
}
