using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Stores;

internal sealed class ProjectTreeNode : BindableObject, IProjectTreeNode
{
    private readonly BPObservableCollection<IProjectTreeNode> _children = [];

    public required string Name { get; init; }

    public string? Extension { get; init; }

    public required string FullPath { get; init; }

    public required bool IsFolder { get; init; }

    public IReadOnlyCollection<IProjectTreeNode> Children => _children;

    public bool HasChildren => Children.Count > 0;

    IReadOnlyCollection<IProjectTreeNode> IProjectTreeNode.Children => Children;

    public void AddChild(IProjectTreeNode child)
    {
        _children.Add(child);
    }

    public void RemoveChild(IProjectTreeNode child)
    {
        _children.Remove(child);
    }
}
