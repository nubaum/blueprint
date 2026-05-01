using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

internal sealed class Transition<T, TArgs> : ITransition<T, TArgs>
{
    private readonly IReadOnlyList<ITransitionStep<T, TArgs>> _steps;

    internal Transition(IReadOnlyList<ITransitionStep<T, TArgs>> steps)
    {
        _steps = steps;
    }

    public void Invoke(T target, TArgs args, [CallerMemberName] string callerName = "")
    {
        var pendingActions = new List<Action<T, TArgs>>();

        foreach (ITransitionStep<T, TArgs> step in _steps)
        {
            switch (step)
            {
                case GuardStep<T, TArgs> g:
                    {
                        if (!g.Guard(target, args))
                        {
                            DomainNotifications.Current.Add(new Notification
                            {
                                Kind = NotificationKind.Error,
                                Message = g.Message,
                                TransitionName = callerName
                            });
                            return;
                        }

                        foreach (Action<T, TArgs> pending in pendingActions)
                        {
                            pending(target, args);
                        }

                        pendingActions.Clear();
                        break;
                    }

                case GuardAllStep<T, TArgs> ga:
                    {
                        var failures = ga.Guards.Where(g => !g.Guard(target, args)).ToList();
                        if (failures.Count > 0)
                        {
                            foreach ((Func<T, TArgs, bool> Guard, string Message) f in failures)
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

                        foreach (Action<T, TArgs> pending in pendingActions)
                        {
                            pending(target, args);
                        }

                        pendingActions.Clear();
                        break;
                    }

                case ActionStep<T, TArgs> a:
                    {
                        pendingActions.Add(a.Action);
                        break;
                    }

                default:
                    // nothing to do here.
                    break;
            }
        }

        foreach (Action<T, TArgs> pending in pendingActions)
        {
            pending(target, args);
        }
    }
}
