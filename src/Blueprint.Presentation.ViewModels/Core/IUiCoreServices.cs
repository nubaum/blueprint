using Blueprint.Abstractions.Application.Workspace;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.ViewModels.Core;

public interface IUiCoreServices
{
    ILoggerFactory LoggerFactory { get; }

    INotificationService NotificationService { get; }
}
