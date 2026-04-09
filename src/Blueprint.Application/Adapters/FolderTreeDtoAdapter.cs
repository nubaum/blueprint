using Blueprint.Application.Abstractions;
using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.Abstractions.Workspace.Models;
using Blueprint.Application.Stores;

namespace Blueprint.Application.Adapters;

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
