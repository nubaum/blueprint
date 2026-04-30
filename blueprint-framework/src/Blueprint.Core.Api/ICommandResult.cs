using Blueprint.Core.Domain;

namespace Blueprint.Core.Api;

public interface ICommandResult<T>
{
    T? Entity { get; }

    IReadOnlyList<Notification> Errors { get; }

    bool HasErrors => Errors.Count > 0;

    bool NotFound { get; }
}
