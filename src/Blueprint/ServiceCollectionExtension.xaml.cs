using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Languages.Adapaters.Actipro;
using Blueprint.Services;
using Blueprint.Services.Interfaces;
using Blueprint.Stores;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Blueprint;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureUI(this IServiceCollection services)
    {
        services.ConfigureCoreServices();
        services.ConfigureServices();
        services.ConfigureLanguages();
        services.AddSingleton<WorskpaceStore>();
        services.AddSingleton<IReadWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());
        services.AddSingleton<IWriteWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());
        return services;
    }

    private static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddHostedService<ApplicationHostService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<ITaskBarService, TaskBarService>();
        services.AddSingleton<INavigationService, NavigationService>();
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IWorkspaceService, WorkspaceService>();
    }
}
