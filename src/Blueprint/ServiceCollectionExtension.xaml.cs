using Blueprint.Application.ViewContracts.Workspace;
using Blueprint.Application.ViewModelContracts;
using Blueprint.Services;
using Blueprint.Stores;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Blueprint;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureUI(this IServiceCollection services)
    {
        ConfigureCoreServices(services);

        services.AddSingleton<WorskpaceStore>();
        services.AddSingleton<IReadWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());
        services.AddSingleton<IWriteWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());
        return services;
    }

    private static void ConfigureCoreServices(IServiceCollection services)
    {
        services.AddHostedService<ApplicationHostService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<ITaskBarService, TaskBarService>();
        services.AddSingleton<INavigationService, NavigationService>();
    }
}
