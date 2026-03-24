using Blueprint.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddSingleton<PathProvider>();
        services.AddSingleton<WorkspaceService>();
        return services;
    }
}
