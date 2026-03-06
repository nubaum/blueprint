using Blueprint.ViewModels.Primitives;
using Wpf.Ui.Controls;

namespace Blueprint.ViewModels.Windows;

public class MainWindowViewModel : NotifyPropertyChangedBase
{
    private readonly List<MenuItem> _trayMenuItems =
    [
        new MenuItem { Header = "Home", Tag = "tray_home" }
    ];

    private readonly string _applicationTitle = "WPF UI - Blueprint";
    private readonly List<object> _footerMenuItems =
    [
        new NavigationViewItem
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = typeof(Views.Pages.SettingsPage)
        }
    ];

    private readonly List<object> _menuItems =
    [
        new NavigationViewItem
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = typeof(Views.Pages.DashboardPage)
        },
        new NavigationViewItem
        {
            Content = "Data",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            TargetPageType = typeof(Views.Pages.DataPage)
        },
        new NavigationViewItem
        {
            Content = "Code",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Code16 },
            TargetPageType = typeof(Views.Pages.CodePage)
        }
    ];

    public string ApplicationTitle => _applicationTitle;

    public IEnumerable<object> MenuItems => _menuItems;

    public IEnumerable<object> FooterMenuItems => _footerMenuItems;

    public IEnumerable<object> TrayMenuItems => _trayMenuItems;
}
