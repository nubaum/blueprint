namespace Blueprint.Domain;

public interface ITransitionRule<TAggregate>
where TAggregate : Aggregate
{
    string Name { get; }
}
