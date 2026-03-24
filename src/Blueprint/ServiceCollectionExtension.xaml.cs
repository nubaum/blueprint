using Blueprint.Abstractions.Application.Languages;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.Handlers.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Languages.Adapaters.Actipro;
using Blueprint.Services;
using Blueprint.Stores;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Blueprint;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureUI(this IServiceCollection services)
    {
        services.ConfigureCoreServices();
        services.ConfigureLanguages();
        services.ConfigureStores();
        return services;
    }

    private static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(OpenDocumentRequestHandler).Assembly);
        });
        services.AddHostedService<ApplicationHostService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<ITaskBarService, TaskBarService>();
        services.AddSingleton<INavigationService, NavigationService>();
    }

    private static void ConfigureStores(this IServiceCollection services)
    {
        services.AddSingleton<WorskpaceStore>();
        services.AddSingleton<IReadWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());
        services.AddSingleton<IWriteWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());

        services.AddSingleton<DocumentStore>();
        services.AddSingleton<IReadDocumentStore>(sp => sp.GetRequiredService<DocumentStore>());
        services.AddSingleton<IWriteDocumentStore>(sp => sp.GetRequiredService<DocumentStore>());
    }
}
