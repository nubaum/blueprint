using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Application.Extensions;

public static class ApplicationConfigurator
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationConfigurator).Assembly));
        return services;
    }
}
