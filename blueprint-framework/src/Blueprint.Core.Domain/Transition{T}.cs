using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

internal sealed class Transition<T> : ITransition<T>
{
    private readonly IReadOnlyList<ITransitionStep<T>> _steps;

    internal Transition(IReadOnlyList<ITransitionStep<T>> steps)
    {
        _steps = steps;
    }

    public void Invoke(T target, [CallerMemberName] string callerName = "")
    {
        var pendingActions = new List<Action<T>>();

        foreach (ITransitionStep<T> step in _steps)
        {
            switch (step)
            {
                case GuardStep<T> g:
                    if (!g.Guard(target))
                    {
                        DomainNotifications.Current.Add(new Notification
                        {
                            Kind = NotificationKind.Error,
                            Message = g.Message,
                            TransitionName = callerName
                        });
                        return;
                    }

                    foreach (Action<T> pending in pendingActions)
                    {
                        pending(target);
                    }

                    pendingActions.Clear();
                    break;

                case GuardAllStep<T> ga:
                    var failures = ga.Guards.Where(g => !g.Guard(target)).ToList();
                    if (failures.Count > 0)
                    {
                        foreach ((Func<T, bool> Guard, string Message) f in failures)
                        {
                            DomainNotifications.Current.Add(new Notification
                            {
                                Kind = NotificationKind.Error,
                                Message = f.Message,
                                TransitionName = callerName
                            });
                        }

                        return;
                    }

                    foreach (Action<T> pending in pendingActions)
                    {
                        pending(target);
                    }

                    pendingActions.Clear();
                    break;

                case ActionStep<T> a:
                    pendingActions.Add(a.Action);
                    break;
                default:
                    // nothing to do here.
                    break;
            }
        }

        foreach (Action<T> pending in pendingActions)
        {
            pending(target);
        }
    }
}
