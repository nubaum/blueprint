using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.Adapters.Abstractions;

public interface IUiCoreServices
{
    ILoggerFactory LoggerFactory { get; }

    INotificationService NotificationService { get; }
}
