using Blueprint.Presentation.ViewModels.Core;
using Wpf.Ui.Controls;

namespace Blueprint.Presentation.ViewModels.Windows;

internal class MainWindowViewModel : NotifyPropertyChangedBase, IMainWindowViewModel
{
    private readonly BPObservableCollection<NavigationViewItem> _menuItems = [];

    private readonly BPObservableCollection<NavigationViewItem> _footerMenuItems = [];

    public string ApplicationTitle { get; } = "Bllueprint";

    public IReadOnlyCollection<NavigationViewItem> MenuItems => _menuItems;

    public IReadOnlyCollection<NavigationViewItem> FooterMenuItems => _footerMenuItems;

    public IReadOnlyCollection<MenuItem> TrayMenuItems { get; } = [
        new MenuItem { Header = "Home", Tag = "tray_home" }
    ];

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
