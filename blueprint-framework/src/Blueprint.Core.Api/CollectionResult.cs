using Blueprint.Core.Domain;

namespace Blueprint.Core.Api;

internal record CollectionResult<T> : ICommandResult<IEnumerable<T>>
{
    public IEnumerable<T>? Entity { get; init; }

    public IReadOnlyList<Notification> Errors { get; init; } = [];

    public bool NotFound { get; init; }

    public static CollectionResult<T> Success(IEnumerable<T> items) =>
        new() { Entity = items };

    public static CollectionResult<T> Missing() =>
        new() { NotFound = true };

    public static CollectionResult<T> Failed(IEnumerable<Notification> errors) =>
        new() { Errors = [.. errors] };
}
