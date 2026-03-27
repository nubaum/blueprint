using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Application.Workspace.Models;

namespace Blueprint.Abstractions.Adapters.Application.Workspace;

public interface IFolderTreeDtoAdapter
{
    IProjectTreeNode ToProjectTreeNode(FolderTreeDto dto);
}
