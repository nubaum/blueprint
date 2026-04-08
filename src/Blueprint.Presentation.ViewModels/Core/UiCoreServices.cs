using Blueprint.Abstractions.Application.Workspace;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.ViewModels.Core;

internal class UiCoreServices(ILoggerFactory loggerFactory, INotificationService notificationService) : IUiCoreServices
{
    public ILoggerFactory LoggerFactory => loggerFactory;

    public INotificationService NotificationService => notificationService;
}
