using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

public abstract class Aggregate<T>
where T : Aggregate<T>
{
    protected static ITransitionBuilder<T> GetTransitionBuilder() => new TransitionBuilder<T>();

    protected static ITransitionBuilder<T, TArgs> GetTransitionBuilder<TArgs>() => new TransitionBuilder<T, TArgs>();

    protected static void AddViolationMessage(string message, [CallerMemberName] string callerName = "")
    {
        DomainNotifications.Current.Add(new Notification
        {
            Kind = NotificationKind.Error,
            Message = message,
            TransitionName = callerName
        });
    }
}
