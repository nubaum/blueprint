using Blueprint.Abstractions.Application.Workspace;
using Microsoft.Extensions.Logging;

namespace Blueprint.Application.Core;

public interface IUiCoreServices
{
    ILoggerFactory LoggerFactory { get; }

    INotificationService NotificationService { get; }
}
