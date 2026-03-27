using ActiproSoftware.Products;
using Blueprint.Abstractions.Application.Workspace;
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

    private async Task HandleActivationAsync()
    {
        serviceProvider.GetRequiredService<IWriteThemeStore>().ChangeThemeCommand.Execute(BlueprintTheme.Dark);
        SplashScreen splashScreen = new("Assets/AppIcoDark.png");
        ActiproLicenseManager.RegisterLicense("Remsoft", "WPF241-LPGQK-GQQNR-VWQ54-YKGG");
        splashScreen.Show(false);
        MainWindow mainWindow = new();
        splashScreen.Close(new TimeSpan(0, 0, 0, 0, 500));
        mainWindow!.Show();
        mainWindow.Navigate(typeof(DashboardPage));
        System.Windows.Application.Current.MainWindow = mainWindow;
    }
}
