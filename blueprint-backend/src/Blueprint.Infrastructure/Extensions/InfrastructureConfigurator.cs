using Blueprint.Application.Abstractions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Infrastructure.Extensions;

public static class InfrastructureConfigurator
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDirectoryScanner, DirectoryScanner>();
        return services;
    }
}
