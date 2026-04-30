namespace Blueprint.Domain;

internal class TransitionRule<TAggregate> : ITransitionRule<TAggregate>, ITransitionRuleExecutor<TAggregate>
where TAggregate : Aggregate
{
    private readonly IReadOnlyList<PropertyMutation<TAggregate>> _mutations;
    private readonly IReadOnlyList<(Func<TAggregate, bool> Check, string Description)> _guards;
    private readonly string _errorMessage;
    private readonly NotificationSeverity _severity;

    public string Name { get; }

    internal TransitionRule(
        string name,
        IReadOnlyList<PropertyMutation<TAggregate>> mutations,
        IReadOnlyList<(Func<TAggregate, bool>, string)> guards,
        string errorMessage,
        NotificationSeverity severity)
    {
        Name = name;
        _mutations = mutations;
        _guards = guards;
        _errorMessage = errorMessage;
        _severity = severity;
    }

    public bool Execute(TAggregate aggregate)
    {
        foreach (var (check, _) in _guards)
        {
            if (!check(aggregate))
            {
                DomainNotifications.Current.Add(Name, _errorMessage, _severity);
                return false;
            }
        }

        foreach (var mutation in _mutations)
            mutation.Apply(aggregate);

        return true;
    }
}
