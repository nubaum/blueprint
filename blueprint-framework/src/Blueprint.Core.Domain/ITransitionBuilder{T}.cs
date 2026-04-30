namespace Blueprint.Core.Domain;

public interface ITransitionBuilder<T>
{
    ITransitionBuilder<T> Do(Action<T> action);

    ITransitionBuilder<T> Requires(Func<T, bool> guard, string message);

    ITransitionBuilder<T> RequiresAll(params (Func<T, bool> Guard, string Message)[] guards);

    ITransition<T> Create();
}
