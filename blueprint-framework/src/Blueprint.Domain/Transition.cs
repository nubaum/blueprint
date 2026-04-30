namespace Blueprint.Domain;

public static class Transition<TAggregate>
where TAggregate : Aggregate
{
    public static ITransitionBuilder<TAggregate> Named(string name)
        => new TransitionBuilder<TAggregate>(name);
}
