using Blueprint.Presentation.Adapters.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.Adapters;

internal class UiCoreServices(ILoggerFactory loggerFactory, INotificationService notificationService) : IUiCoreServices
{
    public ILoggerFactory LoggerFactory => loggerFactory;

    public INotificationService NotificationService => notificationService;
}
