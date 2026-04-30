namespace Blueprint.Domain;

public interface INotificationBag
{
    IReadOnlyList<Notification> All { get; }
    IEnumerable<Notification> Errors { get; }
    IEnumerable<Notification> Warnings { get; }
    bool HasErrors { get; }
    bool IsEmpty { get; }

    void Add(Notification notification);
    void Add(string transitionName, string message, NotificationSeverity severity = NotificationSeverity.Error);
}
