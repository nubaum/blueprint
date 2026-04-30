namespace Blueprint.Core.Domain;

internal sealed class NotificationContext : INotificationContext
{
    private readonly List<Notification> _items = [];

    public IReadOnlyList<Notification> All => _items;

    public IEnumerable<Notification> ValidationErrors =>
        _items.Where(n => n.Kind == NotificationKind.ValidationError);

    public IEnumerable<Notification> Warnings =>
        _items.Where(n => n.Kind == NotificationKind.Warning);

    public IEnumerable<Notification> InternalErrors =>
        _items.Where(n => n.Kind == NotificationKind.InternalError);

    public bool HasErrors =>
        _items.Exists(n => n.Kind == NotificationKind.ValidationError || n.Kind == NotificationKind.InternalError);

    public bool IsEmpty => _items.Count == 0;

    public void Add(Notification notification) =>
        _items.Add(notification);

    public void Add(
        string transitionName,
        string message,
        NotificationKind kind = NotificationKind.ValidationError) =>
        _items.Add(new Notification { TransitionName = transitionName, Message = message, Kind = kind });
}
