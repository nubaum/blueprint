namespace Blueprint.Core.Domain;

internal sealed record GuardStep<T>(Func<T, bool> Guard, string Message) : ITransitionStep<T>;
