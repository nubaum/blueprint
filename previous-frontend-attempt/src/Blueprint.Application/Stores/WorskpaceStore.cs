using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.Adapters;

namespace Blueprint.Application.Stores;

internal class WorskpaceStore : BindableObject, IWriteWorkspaceStore
{
    private readonly BPObservableCollection<object> _menuItems = [];

    private readonly BPObservableCollection<object> _footerMenuItems = [];

    private readonly BPObservableCollection<IWorkspaceItem> _openItems = [];
    private readonly BPObservableCollection<object> _trayMenuItems = [];

    // TODO: This can be placed in a handler or something else, the store shouldn't really do this.
    public WorskpaceStore(IRootNavigationService rootNavigationService)
    {
        _footerMenuItems.Add(rootNavigationService.Settings);
        _menuItems.Add(rootNavigationService.Home);
        _menuItems.Add(rootNavigationService.Data);
        _menuItems.Add(rootNavigationService.Code);
        _trayMenuItems.Add(rootNavigationService.HomeMenu);
    }

    public string ApplicationTitle { get; } = "Bllueprint";

    public IReadOnlyCollection<object> MenuItems => _menuItems;

    public IReadOnlyCollection<object> FooterMenuItems => _footerMenuItems;

    public IReadOnlyCollection<object> TrayMenuItems => _trayMenuItems;

    public ProjectInfo? CurrentProject { get; private set; }

    public IWorkspaceItem? SelectedItem
    {
        get;
        set => SetField(ref field, value);
    }

    public IReadOnlyCollection<IWorkspaceItem> OpenItems => _openItems;

    public void AddItem(IWorkspaceItem item)
    {
        _openItems.Add(item);
        SelectedItem = item;
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

    public void AddEditor(string caption, object editor)
    {
        AddItem(new TabContent { Caption = caption, Content = editor, Kind = WorkspaceItemKind.Doucument });
    }
}
