namespace Blueprint.Core.Domain;

internal sealed class TransitionBuilder<T> : ITransitionBuilder<T>
{
    private readonly List<ITransitionStep<T>> _steps = [];

    public ITransitionBuilder<T> Requires(Func<T, bool> guard, string message)
    {
        _steps.Add(new GuardStep<T>(guard, message));
        return this;
    }

    public ITransitionBuilder<T> RequiresAll(params (Func<T, bool> Guard, string Message)[] guards)
    {
        _steps.Add(new GuardAllStep<T>(guards));
        return this;
    }

    public ITransitionBuilder<T> Do(Action<T> action)
    {
        _steps.Add(new ActionStep<T>(action));
        return this;
    }

    public ITransition<T> Create() => new Transition<T>(_steps);
}
