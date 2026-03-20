using System.Collections.ObjectModel;
using Blueprint.Presentation.ViewModels.Core;
using Wpf.Ui.Controls;

namespace Blueprint.Presentation.ViewModels.Windows;

public class MainWindowViewModel : NotifyPropertyChangedBase
{
    public string ApplicationTitle { get; } = "Bllueprint";

    public ObservableCollection<NavigationViewItem> MenuItems { get; } = [];

    public ObservableCollection<NavigationViewItem> FooterMenuItems { get; } = [];

    public ObservableCollection<MenuItem> TrayMenuItems { get; } = [
        new MenuItem { Header = "Home", Tag = "tray_home" }
    ];

    public void Initialize(Type settingsPageType, Type dashboardPageType, Type dataPageType, Type codePageType)
    {
        FooterMenuItems.Add(new NavigationViewItem
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = settingsPageType
        });
        MenuItems.Add(
        new NavigationViewItem
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = dashboardPageType
        });
        MenuItems.Add(
        new NavigationViewItem
        {
            Content = "Data",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            TargetPageType = dataPageType
        });
        MenuItems.Add(
        new NavigationViewItem
        {
            Content = "Code",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Code16 },
            TargetPageType = codePageType
        });
    }
}
