namespace Blueprint.Domain;

public readonly record struct Notification
{
    public required string TransitionName { get; init; }
    public required string Message { get; init; }
    public required NotificationSeverity Severity { get; init; }
}
