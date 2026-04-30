namespace Blueprint.Core.Api;

internal sealed class HandlerPipeline<T>(
    List<PipelineStep> steps,
    PipelineContext ctx) : IHandlerPipeline<T>
{
    public IWithPipeline<T> WithCheck(Func<T, bool> predicate)
        => new WithPipeline<T>(predicate, steps, ctx);

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask)
    {
        steps.Add(async _ =>
        {
            return ctx.Failed ? default : (object?)await entityTask();
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<T, Task<TNext?>> entityTask)
    {
        steps.Add(async input =>
        {
            if (ctx.Failed)
            {
                return default;
            }

            var entity = (T?)input;
            return entity is null ? default : (object?)await entityTask(entity);
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }

    public IHandlerPipeline<T> Invoke(Action<T> transition)
    {
        steps.Add(input =>
        {
            if (ctx.Failed)
            {
                return Task.FromResult(input);
            }

            var entity = (T?)input;
            if (entity is null)
            {
                return Task.FromResult<object?>(null);
            }

            transition(entity);
            return Task.FromResult<object?>(entity);
        });

        return this;
    }

    public IHandlerPipeline<T> Invoke(Func<Task> guardTask)
    {
        steps.Add(async input =>
        {
            if (ctx.Failed)
            {
                return default;
            }

            var entity = (T?)input;
            if (entity is null)
            {
                return default;
            }

            try
            {
                await guardTask();
            }

            // TODO: Do we really need this or should it be treated as 400?
            catch (Exception ex)
            {
                ctx.Fail(new FailureDetail
                {
                    Message = $"An unexpected error occurred: {ex.Message}",
                    TransitionName = "Pipeline internal",
                    Kind = Domain.NotificationKind.InternalError
                });

                return default;
            }

            return (object?)entity;
        });

        return this;
    }

    public IHandlerPipeline<T> Save(Func<T, Task> persist)
    {
        steps.Add(async input =>
        {
            if (ctx.Failed)
            {
                return default;
            }

            var entity = (T?)input;
            if (entity is null)
            {
                return default;
            }

            await persist(entity);
            return (object?)entity;
        });

        return this;
    }

    public async Task<ICommandResult<T>> ToResultAsync()
    {
        object? current = null;

        foreach (PipelineStep step in steps)
        {
            current = await step(current);

            if (ctx.Failed)
            {
                return CommandResult<T>.Failed(ctx.Notifications.ValidationErrors);
            }

            if (current is null)
            {
                return CommandResult<T>.Missing();
            }
        }

        return CommandResult<T>.Success((T)current!);
    }
}
