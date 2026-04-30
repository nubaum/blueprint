namespace Blueprint.Domain;

public sealed class NotificationBag
{
    private readonly List<Notification> _items = [];

    public IReadOnlyList<Notification> All => _items;

    public IEnumerable<Notification> Errors =>
        _items.Where(n => n.Severity == NotificationSeverity.Error);

    public IEnumerable<Notification> Warnings =>
        _items.Where(n => n.Severity == NotificationSeverity.Warning);

    public bool HasErrors =>
        _items.Any(n => n.Severity == NotificationSeverity.Error);

    public bool IsEmpty => _items.Count == 0;

    public void Add(Notification notification) =>
        _items.Add(notification);

    public void Add(
        string transitionName,
        string message,
        NotificationSeverity severity = NotificationSeverity.Error) =>
        _items.Add(new Notification { TransitionName = transitionName, Message = message, Severity = severity });
}
