namespace Blueprint.Core.Domain;

internal sealed class TransitionBuilder<T> : ITransitionBuilder<T>
{
    private readonly List<(Func<T, bool>? Guard, string? Message, Action<T>? Action)> _steps = new();

    public ITransitionBuilder<T> Requires(Func<T, bool> guard, string message)
    {
        _steps.Add((guard, message, null));
        return this;
    }

    public ITransitionBuilder<T> Do(Action<T> action)
    {
        _steps.Add((null, null, action));
        return this;
    }

    public Transition<T> Create() => new(_steps);
}
