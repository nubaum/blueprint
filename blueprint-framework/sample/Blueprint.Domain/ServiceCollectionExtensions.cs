using Blueprint.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDomain(this IServiceCollection services)
    {
        services.AddBlueprintDomainServices();
        return services;
    }
}
