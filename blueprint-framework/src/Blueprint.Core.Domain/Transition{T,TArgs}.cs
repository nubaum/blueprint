using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

public sealed class Transition<T, TArgs>
{
    private readonly IReadOnlyList<(Func<T, TArgs, bool>? Guard, string? Message, Action<T, TArgs>? Action)> _steps;

    internal Transition(IReadOnlyList<(Func<T, TArgs, bool>? Input, string? Message, Action<T, TArgs>? Action)> steps)
    {
        _steps = steps;
    }

    public void Invoke(T target, TArgs args, [CallerMemberName] string callerName = "")
    {
        var pendingActions = new List<Action<T, TArgs>>();

        foreach ((Func<T, TArgs, bool>? guard, string? message, Action<T, TArgs>? action) in _steps)
        {
            if (guard != null)
            {
                if (!guard(target, args))
                {
                    DomainNotifications.Current.Add(new Notification
                    {
                        Kind = NotificationKind.ValidationError,
                        Message = message!,
                        TransitionName = callerName
                    });
                    return;
                }

                foreach (Action<T, TArgs> pending in pendingActions)
                {
                    pending(target, args);
                }

                pendingActions.Clear();
            }
            else
            {
                pendingActions.Add(action!);
            }
        }

        foreach (Action<T, TArgs> pending in pendingActions)
        {
            pending(target, args);
        }
    }
}
