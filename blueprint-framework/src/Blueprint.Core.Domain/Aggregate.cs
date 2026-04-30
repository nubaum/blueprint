using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

public abstract class Aggregate<T>
where T : Aggregate<T>
{
    protected static ITransitionBuilder<T> TransitionBuilder { get; } = new TransitionBuilder<T>();

    protected static ITransitionBuilder<T, TArgs> GetInputTransition<TArgs>() => new TransitionBuilder<T, TArgs>();

    protected static void AddViolationMessage(string message, [CallerMemberName] string callerName = "")
    {
        DomainNotifications.Current.Add(new Notification
        {
            Kind = NotificationKind.ValidationError,
            Message = message,
            TransitionName = callerName
        });
    }
}
