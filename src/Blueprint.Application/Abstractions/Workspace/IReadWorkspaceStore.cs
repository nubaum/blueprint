namespace Blueprint.Application.Abstractions.Workspace;

public interface IReadWorkspaceStore
{
    IWorkspaceItem? SelectedItem { get; }

    public ProjectInfo? CurrentProject { get; }

    public IReadOnlyCollection<IWorkspaceItem> OpenItems { get; }

    public IReadOnlyCollection<object> MenuItems { get; }

    public IReadOnlyCollection<object> FooterMenuItems { get; }

    public IReadOnlyCollection<object> TrayMenuItems { get; }
}
