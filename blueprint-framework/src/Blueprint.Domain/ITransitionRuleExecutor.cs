namespace Blueprint.Domain;

internal interface ITransitionRuleExecutor<TAggregate>
where TAggregate : Aggregate
{
    bool Execute(TAggregate aggregate);
}