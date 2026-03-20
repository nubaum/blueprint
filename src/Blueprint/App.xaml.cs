using System.Windows.Threading;
using Blueprint.Application;
using Blueprint.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blueprint;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services
                .ConfigureUI()
                .ConfigureViewModels()
                .ConfigureApplication();
        }).Build();

    public static IServiceProvider Services
    {
        get { return _host.Services; }
    }

    public static T GetService<T>()
    where T : notnull
        => Services.GetRequiredService<T>();

    public static object GetService(Type serviceType)
        => Services.GetRequiredService(serviceType);

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _ = _host.StartAsync();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        _ = StopAsync();
    }

    private static async Task StopAsync()
    {
        await _host.StopAsync();
        _host.Dispose();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
    }
}
