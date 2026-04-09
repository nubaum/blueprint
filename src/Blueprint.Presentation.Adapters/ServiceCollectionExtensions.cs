using Blueprint.Application.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Presentation.Adapters;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureWpfAdapter(this IServiceCollection services)
    {
        services.AddSingleton<IUiCoreServices, UiCoreServices>();
        return services;
    }
}
