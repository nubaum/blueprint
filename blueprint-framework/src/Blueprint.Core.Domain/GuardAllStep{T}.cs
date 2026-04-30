namespace Blueprint.Core.Domain;

internal sealed record GuardAllStep<T>(IReadOnlyList<(Func<T, bool> Guard, string Message)> Guards) : ITransitionStep<T>;
