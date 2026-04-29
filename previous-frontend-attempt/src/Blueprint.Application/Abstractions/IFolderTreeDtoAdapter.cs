using Blueprint.Application.Abstractions.Infrastructure.Models;
using Blueprint.Application.Abstractions.Workspace;

namespace Blueprint.Application.Abstractions;

public interface IFolderTreeDtoAdapter
{
    IProjectTreeNode ToProjectTreeNode(FileSystemItem fileSystemItem);
}
