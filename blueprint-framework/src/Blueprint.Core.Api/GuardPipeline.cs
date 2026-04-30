namespace Blueprint.Core.Api;

internal sealed class GuardPipeline(
    Func<Task<bool>> guardTask,
    List<PipelineStep> steps,
    PipelineContext ctx) : IGuardPipeline
{
    public IGuardWithPipeline With(Func<bool, bool> predicate)
        => new GuardWithPipeline(guardTask, predicate, steps, ctx);
}
