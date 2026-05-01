namespace Blueprint.Core.Domain;

public interface INotificationContext
{
    IReadOnlyList<Notification> All { get; }

    IEnumerable<Notification> ValidationErrors { get; }

    IEnumerable<Notification> Warnings { get; }

    bool HasErrors { get; }

    bool IsEmpty { get; }

    void Add(Notification notification);

    void Add(string transitionName, string message, NotificationKind kind = NotificationKind.Error);
}
