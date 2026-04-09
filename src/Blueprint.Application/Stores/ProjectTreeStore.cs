using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.Adapters;

namespace Blueprint.Application.Stores;

internal sealed class ProjectTreeStore : BindableObject, IWriteProjectTreeStore
{
    public IProjectTreeNode? Root { get; private set; }

    public void AddChildrenToNode(IProjectTreeNode parent, IProjectTreeNode child)
    {
        if (parent is ProjectTreeNode parentNode && child is ProjectTreeNode childNode)
        {
            parentNode.AddChild(childNode);
        }
    }

    public void RemoveChildFromNode(IProjectTreeNode parent, IProjectTreeNode child)
    {
        if (parent is ProjectTreeNode parentNode && child is ProjectTreeNode childNode)
        {
            parentNode.RemoveChild(childNode);
        }
    }

    public IProjectTreeNode SetRoot(IProjectTreeNode root)
    {
        Root = root;
        OnPropertyChanged(nameof(Root));

        return root;
    }
}
