using Blueprint.Application.InternalAbstractions;
using Blueprint.Views.Pages;
using Blueprint.Views.Windows;
using Wpf.Ui.Controls;

namespace Blueprint.Services;

internal sealed class ViewNavigationHost : IViewNavigationHost
{
    private NavigationView? _navigationView;

    public ViewNavigationHost()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            _navigationView = ((MainWindow)System.Windows.Application.Current.MainWindow).RootNavigation;
        });
    }

    public void NavigateToCode() => Navigate(typeof(CodePage));

    public void NavigateToData() => Navigate(typeof(DataPage));

    public void NavigateToHome() => Navigate(typeof(DashboardPage));

    public void NavigateToSettings() => Navigate(typeof(SettingsPage));

    private void Navigate(Type pageType)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() => _navigationView?.Navigate(pageType));
    }
}
