namespace Blueprint.Domain;

internal sealed class NotificationScope : IDisposable
{
    private readonly NotificationBag? _previous;
    public NotificationBag Bag { get; }

    internal NotificationScope()
    {
        _previous = DomainNotifications._current.Value;
        Bag = new NotificationBag();
        DomainNotifications.SetCurrent(Bag);
    }

    public void Dispose() => DomainNotifications.SetCurrent(_previous ?? new NotificationBag());
}