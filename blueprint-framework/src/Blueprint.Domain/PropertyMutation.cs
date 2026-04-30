namespace Blueprint.Domain;

internal class PropertyMutation<TAggregate>(Action<TAggregate> apply, string description)
{
    public Action<TAggregate> Apply { get; } = apply;
    public string Description { get; } = description;
}
