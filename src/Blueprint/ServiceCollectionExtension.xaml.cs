using Blueprint.Application.Abstractions;
using Blueprint.Application.Abstractions.Languages;
using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Presentation.Adapters.Abstractions;
using Blueprint.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Blueprint;

// TODO: Split this into samller classes.
internal static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureUI(this IServiceCollection services)
    {
        services
            .ConfigureCoreServices()
            .ConfigureServices();

        return services;
    }

    private static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DataColor).Assembly);
        });
        services.AddHostedService<ApplicationHostService>();
        services.AddSingleton<ITaskBarService, TaskBarService>();
        services.AddSingleton<IFolderPicker, FolderPicker>();
        services.AddSingleton<ISnackbarService, SnackbarService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddConfiguration();
        return services;
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IViewNavigationHost, ViewNavigationHost>();
        services.AddSingleton<IRandomColorGenerator, RandomColorGenerator>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IBlueprintThemeService, BlueprintThemeService>();
        services.AddSingleton<ICodeEditorProvider, CodeEditorProvider>();
        services.AddSingleton<IRootNavigationService, RootNavigationService>();
        services.AddSingleton<IDocumentLoader, DocumentLoader>();
        return services;
    }

    private static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets(typeof(App).Assembly, optional: true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        return services;
    }
}
