namespace Blueprint.Domain;

internal static class DomainNotifications
{
    internal static readonly AsyncLocal<NotificationBag?> _current = new();

    public static NotificationBag Current
        => _current.Value ??= new NotificationBag();

    internal static void SetCurrent(NotificationBag bag)
        => _current.Value = bag;
}
