using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Views.Pages;
using Wpf.Ui.Controls;

namespace Blueprint.Services;

public class RootNavigationService : IRootNavigationService
{
    public object Settings { get; } = new NavigationViewItem
    {
        Content = "Settings",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
        TargetPageType = typeof(SettingsPage)
    };

    public object Home { get; } = new NavigationViewItem
    {
        Content = "Home",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
        TargetPageType = typeof(DashboardPage)
    };

    public object Data { get; } = new NavigationViewItem
    {
        Content = "Data",
        Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
        TargetPageType = typeof(DataPage)
    };

    public object Code { get; } = new NavigationViewItem
    {
        Content = "Code",
        Icon = new SymbolIcon { Symbol = SymbolRegular.Code16 },
        TargetPageType = typeof(CodePage)
    };

    public object HomeMenu { get; } = new MenuItem { Header = "Home", Tag = "tray_home" };
}
