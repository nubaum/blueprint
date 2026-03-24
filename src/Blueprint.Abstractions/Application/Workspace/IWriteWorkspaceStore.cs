namespace Blueprint.Abstractions.Application.Workspace;

public interface IWriteWorkspaceStore
{
    void SetCurrentProject(ProjectInfo projectInfo);

    void ClearItems();

    void ClearItems(Predicate<IWorkspaceItem> criteria);

    void AddItems(IEnumerable<IWorkspaceItem> items);

    void AddItem(IWorkspaceItem item);
}
