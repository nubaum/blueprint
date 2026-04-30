namespace Blueprint.Core.Domain;

internal sealed class NotificationScope : IDisposable
{
    private readonly NotificationContext? _previous;

    internal NotificationScope()
    {
        _previous = DomainNotifications._current.Value;
        NotificationContext current = new();
        NotificationContext = current;
        DomainNotifications.SetCurrent(current);
    }

    public INotificationContext NotificationContext { get; }

    public void Dispose() => DomainNotifications.SetCurrent(_previous ?? new NotificationContext());
}
