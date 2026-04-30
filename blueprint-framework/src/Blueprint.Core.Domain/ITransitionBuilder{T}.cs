namespace Blueprint.Core.Domain;

public interface ITransitionBuilder<T>
{
    Transition<T> Create();

    ITransitionBuilder<T> Do(Action<T> action);

    ITransitionBuilder<T> Requires(Func<T, bool> guard, string message);
}
