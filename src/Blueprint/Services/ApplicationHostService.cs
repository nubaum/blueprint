using ActiproSoftware.Products;
using Blueprint.Application.Abstractions.Languages;
using Blueprint.Application.Requests;
using Blueprint.Views.Pages;
using Blueprint.Views.Windows;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blueprint.Services;

internal class ApplicationHostService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    private static void InitiateMainWindow()
    {
        SplashScreen splashScreen = new("Assets/AppIcoDark.png");
        splashScreen.Show(false);
        MainWindow mainWindow = new() { WindowState = WindowState.Normal };
        splashScreen.Close(new TimeSpan(0, 0, 0, 0, 500));
        mainWindow.Show();
        mainWindow.Navigate(typeof(DashboardPage));
        System.Windows.Application.Current.MainWindow = mainWindow;
    }

    private async Task HandleActivationAsync()
    {
        await SetDefaultThemeAsync();
        RegisterActipro();
        InitiateMainWindow();
    }

    private void RegisterActipro()
    {
        IActiproLicenseProvider licenseProvider = serviceProvider.GetRequiredService<IActiproLicenseProvider>();
        ActiproLicenseManager.RegisterLicense(licenseProvider.Licensee, licenseProvider.LicenseKey);
    }

    private async Task SetDefaultThemeAsync()
        => await serviceProvider.GetRequiredService<IMediator>().Send(new SetDefaultThemeRequest());
}
