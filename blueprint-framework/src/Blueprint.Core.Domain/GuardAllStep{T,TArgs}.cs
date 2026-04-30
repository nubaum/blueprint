namespace Blueprint.Core.Domain;

internal sealed record GuardAllStep<T, TArgs>(IReadOnlyList<(Func<T, TArgs, bool> Guard, string Message)> Guards) : ITransitionStep<T, TArgs>;
