namespace Blueprint.Abstractions.Application.Workspace.Models;

public record Notification
{
    public required string Caption { get; init; }

    public required string Message { get; init; }

    public required NotificationKind Kind { get; init; }

    public required int LifespanSeconds { get; init; }
}
