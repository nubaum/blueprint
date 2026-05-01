using Blueprint.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ITaskRepository, TaskRepository>();
        return services;
    }
}
