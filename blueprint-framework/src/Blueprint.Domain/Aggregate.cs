namespace Blueprint.Domain;

public abstract class Aggregate
{
    protected void ExecuteTransition<TAggregate>(ITransitionRule<TAggregate> transition)
        where TAggregate : Aggregate
    {
        if (transition is not ITransitionRuleExecutor<TAggregate> executor)
            throw new InvalidOperationException($"Transition '{transition.Name}' does not implement ITransitionRuleExecutor.");

        if (this is not TAggregate aggregate)
            throw new InvalidOperationException($"Transition '{transition.Name}' was applied to the wrong aggregate type.");

        executor.Execute(aggregate);
    }
}