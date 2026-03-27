using Blueprint.Abstractions.Infrastructure;
using Blueprint.Infrastructure.IOManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDirectoryInfoProvider, DirectoryInfoProvider>();
        return services;
    }
}
