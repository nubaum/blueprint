namespace Blueprint.Core.Domain;

internal static class DomainNotifications
{
    internal static readonly AsyncLocal<NotificationContext?> _current = new();

    public static NotificationContext Current
        => _current.Value ??= new NotificationContext();

    internal static void SetCurrent(NotificationContext notificationContext)
        => _current.Value = notificationContext;
}
