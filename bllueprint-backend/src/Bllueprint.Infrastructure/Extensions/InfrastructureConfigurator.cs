using Bllueprint.Application.Abstractions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Infrastructure.Extensions;

public static class InfrastructureConfigurator
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDirectoryScanner, DirectoryScanner>();
        return services;
    }
}
