using Blueprint.Application.Abstractions.Languages;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Languages.Licensing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLicenseProvider(this IServiceCollection services)
    {
        services.AddSingleton<IActiproLicenseProvider, ActiproLicenseProvider>();
        return services;
    }
}
