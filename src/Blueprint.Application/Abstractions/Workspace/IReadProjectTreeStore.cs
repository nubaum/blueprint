namespace Blueprint.Abstractions.Application.Workspace;

public interface IReadProjectTreeStore
{
    public IProjectTreeNode? Root { get; }
}
