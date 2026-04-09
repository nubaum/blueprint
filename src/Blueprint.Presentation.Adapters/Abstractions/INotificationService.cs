namespace Blueprint.Presentation.Adapters.Abstractions;

public interface INotificationService
{
    Task ShowAsync(Notification notification);
}
