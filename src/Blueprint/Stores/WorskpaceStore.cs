using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Services.Interfaces;

namespace Blueprint.Stores;

internal class WorskpaceStore : NotifyPropertyChangedBase, IReadWorkspaceStore, IWriteWorkspaceStore
{
    private readonly List<IWorkspaceItem> _openItems = [];

    public ProjectInfo? CurrentProject { get; private set; }

    public IReadOnlyCollection<IWorkspaceItem> OpenItems => _openItems;

    public void AddItem(IWorkspaceItem item)
    {
        _openItems.Add(item);
    }

    public void AddItems(IEnumerable<IWorkspaceItem> items)
    {
        _openItems.AddRange(items);
    }

    public void ClearItems()
    {
        _openItems.Clear();
    }

    public void ClearItems(Predicate<IWorkspaceItem> criteria)
    {
        _openItems.RemoveAll(criteria);
    }

    public void SetCurrentProject(ProjectInfo projectInfo)
    {
        CurrentProject = projectInfo;
    }
}
