using System.Linq.Expressions;

namespace Blueprint.Domain;

internal class TransitionBuilder<TAggregate> : ITransitionBuilder<TAggregate>
where TAggregate : Aggregate
{
    private readonly string _name;
    private readonly List<PropertyMutation<TAggregate>> _mutations = [];
    private readonly List<(Func<TAggregate, bool> Check, string Description)> _guards = [];
    private string _errorMessage = "Transition is not allowed in the current state.";
    private NotificationSeverity _severity = NotificationSeverity.Error;

    internal TransitionBuilder(string name) => _name = name;

    public ITransitionBuilder<TAggregate> Set<TValue>(
        Expression<Func<TAggregate, TValue>> property, TValue value)
    {
        var setter = BuildSetter(property);
        _mutations.Add(new PropertyMutation<TAggregate>(
            a => setter(a, value),
            $"{GetMemberName(property)} = {value}"));
        return this;
    }

    public ITransitionBuilder<TAggregate> Set<TValue>(
        Expression<Func<TAggregate, TValue>> property, Func<TValue> valueFactory)
    {
        var setter = BuildSetter(property);
        _mutations.Add(new PropertyMutation<TAggregate>(
            a => setter(a, valueFactory()),
            $"{GetMemberName(property)} = <computed>"));
        return this;
    }

    public ITransitionBuilder<TAggregate> Requires(
        Expression<Func<TAggregate, bool>> condition)
    {
        var compiled = condition.Compile();
        _guards.Add((compiled, condition.ToString()));
        return this;
    }

    public ITransitionBuilder<TAggregate> WithErrorMessage(
        string message,
        NotificationSeverity severity = NotificationSeverity.Error)
    {
        _errorMessage = message;
        _severity = severity;
        return this;
    }

    public ITransitionRule<TAggregate> Build() =>
        new TransitionRule<TAggregate>(
            _name,
            _mutations.AsReadOnly(),
            _guards.AsReadOnly(),
            _errorMessage,
            _severity);

    private static Action<TAggregate, TValue> BuildSetter<TValue>(
        Expression<Func<TAggregate, TValue>> selector)
    {
        if (selector.Body is not MemberExpression member)
            throw new ArgumentException("Expression must be a property accessor.");

        var param = selector.Parameters[0];
        var valueParam = Expression.Parameter(typeof(TValue), "value");
        var assign = Expression.Assign(member, valueParam);
        return Expression.Lambda<Action<TAggregate, TValue>>(assign, param, valueParam)
                         .Compile();
    }

    private static string GetMemberName<TValue>(Expression<Func<TAggregate, TValue>> expr)
        => expr.Body is MemberExpression m ? m.Member.Name : expr.ToString();
}
