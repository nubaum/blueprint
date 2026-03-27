using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.Handlers.Workspace;
using Blueprint.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPathProvider, PathProvider>();
        services.AddSingleton<WorkspaceService>();
        services.AddSingleton<FolderTreeBuilder>();
        return services;
    }
}
