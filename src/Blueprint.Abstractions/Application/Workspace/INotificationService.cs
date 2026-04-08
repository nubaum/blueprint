using Blueprint.Abstractions.Application.Workspace.Models;

namespace Blueprint.Abstractions.Application.Workspace;

public interface INotificationService
{
    Task ShowAsync(Notification notification);
}
