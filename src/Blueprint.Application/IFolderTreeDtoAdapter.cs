using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.Abstractions.Workspace.Models;

namespace Blueprint.Application;

public interface IFolderTreeDtoAdapter
{
    IProjectTreeNode ToProjectTreeNode(FolderTreeDto dto);
}
