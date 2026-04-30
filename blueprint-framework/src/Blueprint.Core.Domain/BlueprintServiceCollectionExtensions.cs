using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Core.Domain;

public static class BlueprintServiceCollectionExtensions
{
    public static IServiceCollection AddBlueprintDomainServices(
        this IServiceCollection services)
    {
        services.AddScoped<INotificationContext>(sp =>
        {
            var notificationContext = new NotificationContext();
            DomainNotifications.SetCurrent(notificationContext);
            return notificationContext;
        });

        return services;
    }
}
