using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.Abstractions.Workspace.Models;

namespace Blueprint.Application.Abstractions;

public interface IFolderTreeDtoAdapter
{
    IProjectTreeNode ToProjectTreeNode(FolderTreeDto dto);
}
