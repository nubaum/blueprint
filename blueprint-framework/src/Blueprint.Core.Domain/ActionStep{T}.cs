namespace Blueprint.Core.Domain;

internal sealed record ActionStep<T>(Action<T> Action) : ITransitionStep<T>;
