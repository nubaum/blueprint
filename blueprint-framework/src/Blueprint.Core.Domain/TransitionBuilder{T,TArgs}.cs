namespace Blueprint.Core.Domain;

internal sealed class TransitionBuilder<T, TArgs> : ITransitionBuilder<T, TArgs>
{
    private readonly List<ITransitionStep<T, TArgs>> _steps = [];

    public ITransitionBuilder<T, TArgs> Requires(Func<T, TArgs, bool> guard, string message)
    {
        _steps.Add(new GuardStep<T, TArgs>(guard, message));
        return this;
    }

    public ITransitionBuilder<T, TArgs> RequiresAll(params (Func<T, TArgs, bool> Guard, string Message)[] guards)
    {
        _steps.Add(new GuardAllStep<T, TArgs>(guards));
        return this;
    }

    public ITransitionBuilder<T, TArgs> Do(Action<T, TArgs> action)
    {
        _steps.Add(new ActionStep<T, TArgs>(action));
        return this;
    }

    public ITransition<T, TArgs> Create() => new Transition<T, TArgs>(_steps);
}
