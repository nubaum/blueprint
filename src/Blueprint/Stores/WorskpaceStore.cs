using Blueprint.Application.ViewContracts.Workspace;
using Blueprint.Application.ViewModelContracts;
using Blueprint.Application.ViewModelContracts.Models;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Stores;

internal class WorskpaceStore : NotifyPropertyChangedBase, IReadWorkspaceStore, IWriteWorkspaceStore
{
    private readonly List<WorkspaceItem> _openItems = [];
    private ProjectInfo? _currentProject;

    public ProjectInfo? CurrentProject
    {
        get => _currentProject;
    }

    public IReadOnlyCollection<WorkspaceItem> OpenItems => _openItems;

    public void AddItem(WorkspaceItem item)
    {
        _openItems.Add(item);
    }

    public void AddItems(IEnumerable<WorkspaceItem> items)
    {
        _openItems.AddRange(items);
    }

    public void ClearItems()
    {
        _openItems.Clear();
    }

    public void ClearItems(Predicate<WorkspaceItem> criteria)
    {
        _openItems.RemoveAll(criteria);
    }

    public void SetCurrentProject(ProjectInfo projectInfo)
    {
        _currentProject = projectInfo;
    }
}
