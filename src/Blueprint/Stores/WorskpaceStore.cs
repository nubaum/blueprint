using ActiproSoftware.Text.Implementation;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;
using Blueprint.Views.Models;
using Blueprint.Views.Pages;
using Blueprint.Views.UserControls;
using Wpf.Ui.Controls;

namespace Blueprint.Stores;

internal class WorskpaceStore : BindableObject, IWriteWorkspaceStore
{
    private readonly BPObservableCollection<NavigationViewItem> _menuItems = [];

    private readonly BPObservableCollection<NavigationViewItem> _footerMenuItems = [];

    private readonly BPObservableCollection<IWorkspaceItem> _openItems = [];

    private readonly NavigationViewItem _settingsNav = new()
    {
        Content = "Settings",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
        TargetPageType = typeof(SettingsPage)
    };

    private readonly NavigationViewItem _homeNav = new()
    {
        Content = "Home",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
        TargetPageType = typeof(DashboardPage)
    };

    private readonly NavigationViewItem _dataNav = new()
    {
        Content = "Data",
        Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
        TargetPageType = typeof(DataPage)
    };

    private readonly NavigationViewItem _codeNav = new()
    {
        Content = "Code",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Code16 },
        TargetPageType = typeof(CodePage)
    };

    public WorskpaceStore()
    {
        _footerMenuItems.Add(_settingsNav);
        _menuItems.Add(_homeNav);
        _menuItems.Add(_dataNav);
        _menuItems.Add(_codeNav);
    }

    public string ApplicationTitle { get; } = "Bllueprint";

    public IReadOnlyCollection<object> MenuItems => _menuItems;

    public IReadOnlyCollection<object> FooterMenuItems => _footerMenuItems;

    public IReadOnlyCollection<object> TrayMenuItems { get; } = [
            new MenuItem { Header = "Home", Tag = "tray_home" }
        ];

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

    public void AddEditor(string caption, object document)
    {
        var result = new BlueLangEditor();
        if (document is EditorDocument doc && result.DataContext is IBlueLangEditorViewModel viewModel)
        {
            viewModel.Document = doc;
        }

        AddItem(new TabContent { Caption = caption, Content = result, Kind = WorkspaceItemKind.Doucument });
    }
}
