using Blueprint.Core.Domain;

namespace Blueprint.Core.Application;

public interface ICommandResult<T>
{
    T? Entity { get; }

    IReadOnlyList<Notification> Errors { get; }

    bool HasErrors => Errors.Count > 0;

    bool NotFound { get; }
}
