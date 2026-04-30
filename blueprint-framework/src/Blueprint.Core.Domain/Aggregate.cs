using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

public abstract class Aggregate
{
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
