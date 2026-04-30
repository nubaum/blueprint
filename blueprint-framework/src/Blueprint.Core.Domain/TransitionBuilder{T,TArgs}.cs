namespace Blueprint.Core.Domain;

internal sealed class TransitionBuilder<T, TArgs> : ITransitionBuilder<T, TArgs>
{
    private readonly List<(Func<T, TArgs, bool>? Guard, string? Message, Action<T, TArgs>? Action)> _steps = new();

    public ITransitionBuilder<T, TArgs> Requires(Func<T, TArgs, bool> guard, string message)
    {
        _steps.Add((guard, message, null));
        return this;
    }

    public ITransitionBuilder<T, TArgs> Do(Action<T, TArgs> action)
    {
        _steps.Add((null, null, action));
        return this;
    }

    public Transition<T, TArgs> Create() => new(_steps);
}
