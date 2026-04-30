using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

public sealed class Transition<T>
{
    private readonly IReadOnlyList<(Func<T, bool>? Guard, string? Message, Action<T>? Action)> _steps;

    internal Transition(IReadOnlyList<(Func<T, bool>? Condition, string? Message, Action<T>? Action)> steps)
    {
        _steps = steps;
    }

    public void Invoke(T target, [CallerMemberName] string callerName = "")
    {
        var pendingActions = new List<Action<T>>();

        foreach ((Func<T, bool>? guard, string? message, Action<T>? action) in _steps)
        {
            if (guard != null)
            {
                if (!guard(target))
                {
                    DomainNotifications.Current.Add(new Notification
                    {
                        Kind = NotificationKind.ValidationError,
                        Message = message!,
                        TransitionName = callerName
                    });
                    return;
                }

                foreach (Action<T> pending in pendingActions)
                {
                    pending(target);
                }

                pendingActions.Clear();
            }
            else
            {
                pendingActions.Add(action!);
            }
        }

        foreach (Action<T> pending in pendingActions)
        {
            pending(target);
        }
    }
}
