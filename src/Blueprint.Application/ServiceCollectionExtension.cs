using Blueprint.Abstractions.Adapters.Application.Workspace;
using Blueprint.Abstractions.Application.Languages;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Adapters.Workspace;
using Blueprint.Application.Handlers.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Application.Services;
using Blueprint.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPathProvider, PathProvider>();
        services.AddSingleton<WorkspaceService>();
        services.AddSingleton<FolderTreeBuilder>();
        services.ConfigureStores();
        services.ConfigureAdapters();
        return services;
    }

    private static IServiceCollection ConfigureStores(this IServiceCollection services)
    {
        services.AddSingleton<WorskpaceStore>();
        services.AddSingleton<IReadWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());
        services.AddSingleton<IWriteWorkspaceStore>(sp => sp.GetRequiredService<WorskpaceStore>());

        services.AddSingleton<DocumentStore>();
        services.AddSingleton<IReadDocumentStore>(sp => sp.GetRequiredService<DocumentStore>());
        services.AddSingleton<IWriteDocumentStore>(sp => sp.GetRequiredService<DocumentStore>());

        services.AddSingleton<ThemeStore>();
        services.AddSingleton<IReadThemeStore>(sp => sp.GetRequiredService<ThemeStore>());
        services.AddSingleton<IWriteThemeStore>(sp => sp.GetRequiredService<ThemeStore>());

        services.AddSingleton<ProjectTreeStore>();
        services.AddSingleton<IReadProjectTreeStore>(sp => sp.GetRequiredService<ProjectTreeStore>());
        services.AddSingleton<IWriteProjectTreeStore>(sp => sp.GetRequiredService<ProjectTreeStore>());
        return services;
    }

    private static IServiceCollection ConfigureAdapters(this IServiceCollection services)
    {
        services.AddSingleton<IFolderTreeDtoAdapter, FolderTreeDtoAdapter>();
        return services;
    }
}
