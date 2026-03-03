using Blueprint.Services;
using Blueprint.ViewModels.Pages;
using Blueprint.ViewModels.UserControls;
using Blueprint.ViewModels.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace Blueprint
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)); })
            .ConfigureServices((context, services) =>
            {
                services.AddNavigationViewPageProvider();

                services.AddHostedService<ApplicationHostService>();

                services.AddSingleton<IThemeService, ThemeService>();

                services.AddSingleton<ITaskBarService, TaskBarService>();

                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<DataViewModel>();
                services.AddSingleton<CodeViewModel>();

            }).Build();

        public static IServiceProvider Services
        {
            get { return _host.Services; }
        }

        public static T GetService<T>() where T : notnull
            => Services.GetRequiredService<T>();

        public static object GetService(Type serviceType)
            => Services.GetRequiredService(serviceType);

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
