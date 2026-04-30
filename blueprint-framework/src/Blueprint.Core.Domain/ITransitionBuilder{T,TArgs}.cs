namespace Blueprint.Core.Domain;

public interface ITransitionBuilder<T, TArgs>
{
    Transition<T, TArgs> Create();

    ITransitionBuilder<T, TArgs> Do(Action<T, TArgs> action);

    ITransitionBuilder<T, TArgs> Requires(Func<T, TArgs, bool> guard, string message);
}
