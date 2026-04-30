using System.Linq.Expressions;

namespace Blueprint.Domain;

public interface ITransitionBuilder<TAggregate>
where TAggregate : Aggregate
{
    ITransitionBuilder<TAggregate> Set<TValue>(
        Expression<Func<TAggregate, TValue>> property, TValue value);

    ITransitionBuilder<TAggregate> Set<TValue>(
        Expression<Func<TAggregate, TValue>> property, Func<TValue> valueFactory);

    ITransitionBuilder<TAggregate> Requires(
        Expression<Func<TAggregate, bool>> condition);

    ITransitionBuilder<TAggregate> WithErrorMessage(
        string message,
        NotificationSeverity severity = NotificationSeverity.Error);

    ITransitionRule<TAggregate> Build();
}
