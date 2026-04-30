namespace Blueprint.Core.Api;

public interface IGuardPipeline
{
    IGuardWithPipeline With(Func<bool, bool> predicate);
}
