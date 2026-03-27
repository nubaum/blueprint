using ActiproSoftware.Products;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Licensing;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Views.Pages;
using Blueprint.Views.Windows;
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
        MainWindow mainWindow = new() { WindowState = WindowState.Maximized };
        splashScreen.Close(new TimeSpan(0, 0, 0, 0, 500));
        mainWindow.Show();
        mainWindow.Navigate(typeof(DashboardPage));
        System.Windows.Application.Current.MainWindow = mainWindow;
    }

    private async Task HandleActivationAsync()
    {
        SetDefaultTheme();
        RegisterActipro();
        InitiateMainWindow();
    }

    private void RegisterActipro()
    {
        IActiproLicenseProvider licenseProvider = serviceProvider.GetRequiredService<IActiproLicenseProvider>();
        ActiproLicenseManager.RegisterLicense(licenseProvider.Licensee, licenseProvider.LicenseKey);
    }

    private void SetDefaultTheme()
        => serviceProvider.GetRequiredService<IWriteThemeStore>().ChangeThemeCommand.Execute(BlueprintTheme.Dark);
}
