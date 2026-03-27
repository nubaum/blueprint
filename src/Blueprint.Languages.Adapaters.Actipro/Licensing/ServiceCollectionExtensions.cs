using Blueprint.Abstractions.Licensing;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Languages.Adapaters.Actipro.Licensing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLicenseProvider(this IServiceCollection services)
    {
        services.AddSingleton<IActiproLicenseProvider, ActiproLicenseProvider>();
        return services;
    }
}
