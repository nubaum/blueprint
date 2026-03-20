using Blueprint.Application.ViewModelContracts.Models;

namespace Blueprint.Application.ViewContracts.Workspace;

public interface IWriteWorkspaceStore
{
    void SetCurrentProject(ProjectInfo projectInfo);

    void ClearItems();

    void ClearItems(Predicate<WorkspaceItem> criteria);

    void AddItems(IEnumerable<WorkspaceItem> items);

    void AddItem(WorkspaceItem item);
}
