namespace Blueprint.Domain;

internal sealed class NotificationScope : IDisposable
{
    private readonly NotificationBag? _previous;
    public INotificationBag Bag { get; }

    internal NotificationScope()
    {
        _previous = DomainNotifications._current.Value;
        NotificationBag current = new();
        Bag = current;
        DomainNotifications.SetCurrent(current);
    }

    public void Dispose() => DomainNotifications.SetCurrent(_previous ?? new NotificationBag());
}