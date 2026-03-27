using System.Windows.Threading;
using Blueprint.Application;
using Blueprint.Infrastructure;
using Blueprint.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blueprint;

public partial class App
{
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services
                .ConfigureInfrastructure()
                .ConfigureUI()
                .ConfigureViewModels()
                .ConfigureApplication();
        })
        .ConfigureLogging()
        .Build();

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
        ILogger logger = Services.GetRequiredService<ILogger<App>>();
        logger?.LogError(e.Exception, "An error occurred in the UI thread.");
        e.Handled = true;
    }
}
