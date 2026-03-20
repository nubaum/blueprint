using System.Collections.ObjectModel;
using Blueprint.Presentation.ViewModels.Core;
using Wpf.Ui.Controls;

namespace Blueprint.Presentation.ViewModels.Windows;

public class MainWindowViewModel : NotifyPropertyChangedBase
{
    private readonly ObservableCollection<MenuItem> _trayMenuItems =
    [
        new MenuItem { Header = "Home", Tag = "tray_home" }
    ];

    private readonly ObservableCollection<NavigationViewItem> _footerMenuItems = [];
    private readonly ObservableCollection<NavigationViewItem> _menuItems = [];

    public string ApplicationTitle { get; } = "Bllueprint";

    public ObservableCollection<NavigationViewItem> MenuItems => _menuItems;

    public ObservableCollection<NavigationViewItem> FooterMenuItems => _footerMenuItems;

    public ObservableCollection<MenuItem> TrayMenuItems => _trayMenuItems;

    public void Initialize(Type settingsPageType, Type dashboardPageType, Type dataPageType, Type codePageType)
    {
        _footerMenuItems.Add(new NavigationViewItem
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = settingsPageType
        });
        _menuItems.Add(
        new NavigationViewItem
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = dashboardPageType
        });
        _menuItems.Add(
        new NavigationViewItem
        {
            Content = "Data",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            TargetPageType = dataPageType
        });
        _menuItems.Add(
        new NavigationViewItem
        {
            Content = "Code",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Code16 },
            TargetPageType = codePageType
        });
    }
}
