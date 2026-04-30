namespace Blueprint.Core.Domain;

public interface ITransitionBuilder<T, TArgs>
{
    ITransitionBuilder<T, TArgs> Do(Action<T, TArgs> action);

    ITransitionBuilder<T, TArgs> Requires(Func<T, TArgs, bool> guard, string message);

    ITransitionBuilder<T, TArgs> RequiresAll(params (Func<T, TArgs, bool> Guard, string Message)[] guards);

    ITransition<T, TArgs> Create();
}
