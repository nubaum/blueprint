using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Application.InternalAbstractions;

internal interface IWriteWorkspaceStore : IReadWorkspaceStore
{
    void SetCurrentProject(ProjectInfo projectInfo);

    void ClearItems();

    void ClearItems(Predicate<IWorkspaceItem> criteria);

    void AddItems(IEnumerable<IWorkspaceItem> items);

    void AddItem(IWorkspaceItem item);

    void AddEditor(string caption, object document);
}
