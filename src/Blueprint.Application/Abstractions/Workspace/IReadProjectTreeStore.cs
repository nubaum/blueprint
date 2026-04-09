namespace Blueprint.Application.Abstractions.Workspace;

public interface IReadProjectTreeStore
{
    public IProjectTreeNode? Root { get; }
}
