using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Domain;

public static class BlueprintServiceCollectionExtensions
{
    public static IServiceCollection AddBlueprintDomainServices(
        this IServiceCollection services)
    {
        services.AddScoped(sp =>
        {
            var bag = new NotificationBag();
            DomainNotifications.SetCurrent(bag);
            return bag;
        });

        return services;
    }
}
