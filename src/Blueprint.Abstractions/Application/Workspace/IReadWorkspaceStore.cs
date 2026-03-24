namespace Blueprint.Abstractions.Application.Workspace;

public interface IReadWorkspaceStore
{
    public ProjectInfo? CurrentProject { get; }

    public IReadOnlyCollection<IWorkspaceItem> OpenItems { get; }
}
