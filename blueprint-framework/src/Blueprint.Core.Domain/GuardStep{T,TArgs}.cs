namespace Blueprint.Core.Domain;

internal sealed record GuardStep<T, TArgs>(Func<T, TArgs, bool> Guard, string Message) : ITransitionStep<T, TArgs>;
