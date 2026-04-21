using Blueprint.Application.Abstractions.Infrastructure;
using Blueprint.Infrastructure.IOManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDirectoryScanner, DirectoryScanner>();
        return services;
    }
}
