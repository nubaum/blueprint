using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Application.InternalAbstractions;

internal interface IWriteProjectTreeStore : IReadProjectTreeStore
{
    public IProjectTreeNode SetRoot(IProjectTreeNode root);

    public void AddChildrenToNode(IProjectTreeNode parent, IProjectTreeNode child);

    public void RemoveChildFromNode(IProjectTreeNode parent, IProjectTreeNode child);
}
