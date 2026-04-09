using Blueprint.Application.Abstractions;
using Blueprint.Application.Abstractions.Languages;
using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.Adapters;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Application.Services;
using Blueprint.Application.Stores;
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
        services.AddStore<WorskpaceStore, IReadWorkspaceStore, IWriteWorkspaceStore>();
        services.AddStore<DocumentStore, IReadDocumentStore, IWriteDocumentStore>();
        services.AddStore<ThemeStore, IReadThemeStore, IWriteThemeStore>();
        services.AddStore<ProjectTreeStore, IReadProjectTreeStore, IWriteProjectTreeStore>();
        return services;
    }

    private static IServiceCollection ConfigureAdapters(this IServiceCollection services)
    {
        services.AddSingleton<IFolderTreeDtoAdapter, FolderTreeDtoAdapter>();
        return services;
    }

    private static IServiceCollection AddStore<TStore, TRead, TWrite>(this IServiceCollection services)
        where TStore : class, TRead, TWrite
        where TRead : class
        where TWrite : class
    {
        services.AddSingleton<TStore>();
        services.AddSingleton<TRead>(sp => sp.GetRequiredService<TStore>());
        services.AddSingleton<TWrite>(sp => sp.GetRequiredService<TStore>());
        return services;
    }
}
