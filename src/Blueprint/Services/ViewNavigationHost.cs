using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.Adapters;
using Blueprint.Views.Pages;
using Wpf.Ui.Controls;

namespace Blueprint.Services;

internal sealed class ViewNavigationHost : IViewNavigationHost
{
    private NavigationView? _navigationView;

    public ViewNavigationHost()
    {
        UIDispatcher.RunOnUiThread(() =>
        {
            _navigationView = new NavigationView();
        });
    }

    public void NavigateToCode() => Navigate(typeof(CodePage));

    public void NavigateToData() => Navigate(typeof(DataPage));

    public void NavigateToHome() => Navigate(typeof(DashboardPage));

    public void NavigateToSettings() => Navigate(typeof(SettingsPage));

    private void Navigate(Type pageType)
    {
        UIDispatcher.RunOnUiThread(() => _navigationView?.Navigate(pageType));
    }
}
