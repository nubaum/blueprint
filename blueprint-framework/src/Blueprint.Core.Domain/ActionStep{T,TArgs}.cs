namespace Blueprint.Core.Domain;

internal sealed record ActionStep<T, TArgs>(Action<T, TArgs> Action) : ITransitionStep<T, TArgs>;
