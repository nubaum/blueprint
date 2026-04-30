namespace Blueprint.Core.Api;

public interface IGuardWithPipeline
{
    IGuardWithPipeline WithMessage(string message);

    IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask);
}
