using Blueprint.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blueprint.Api;

internal record CommandResult<T> : ICommandResult<T>
{
    public T? Entity { get; init; }
    public IReadOnlyList<Notification> Errors { get; init; } = [];
    public bool NotFound { get; init; }

    public static CommandResult<T> Success(T entity) =>
        new() { Entity = entity };

    public static CommandResult<T> Missing() =>
        new() { NotFound = true };

    public static CommandResult<T> Failed(IEnumerable<Notification> errors) =>
        new() { Errors = [.. errors] };
}

internal record CollectionResult<T> : ICommandResult<IEnumerable<T>>
{
    public IEnumerable<T>? Entity { get; init; }
    public IReadOnlyList<Notification> Errors { get; init; } = [];
    public bool NotFound { get; init; }

    public static CollectionResult<T> Success(IEnumerable<T> items) =>
        new() { Entity = items };

    public static CollectionResult<T> Missing() =>
        new() { NotFound = true };

    public static CollectionResult<T> Failed(IEnumerable<Notification> errors) =>
        new() { Errors = [.. errors] };
}

[ApiController]
public abstract class AppController : ControllerBase
{
    private IMediator _mediator = null!;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected Task<IActionResult> SendAsync<T>(IRequest<ICommandResult<T>> command)
        => Mediator
            .Send(command)
            .ToActionResult(this);
}

internal static class CommandResultExtensions
{
    public static async Task<IActionResult> ToActionResult<T>(
        this Task<ICommandResult<T>> resultTask,
        ControllerBase controller)
    {
        var result = await resultTask;

        if (result.NotFound)
            return controller.NotFound();

        if (result.HasErrors)
            return controller.BadRequest(new
            {
                errors = result.Errors.Select(e => new
                {
                    transition = e.TransitionName,
                    message = e.Message,
                    severity = e.Severity.ToString()
                })
            });

        return controller.Ok(result.Entity);
    }
}

public static class PipelineGuard
{
    public static Func<T, bool> NotNull<T>() where T : class => x => x is not null;

    public static Func<T, bool> NotEmpty<T, TKey>(Func<T, TKey> selector)
        where TKey : struct => x => !selector(x).Equals(default(TKey));
}

internal sealed class PipelineContext(INotificationBag notifications)
{
    public INotificationBag Notifications { get; } = notifications;
    public bool Failed => Notifications.HasErrors;

    public void Fail(string message, string? transitionName = null,
                     NotificationSeverity severity = NotificationSeverity.Error)
    {
        Notifications.Add(new Notification
        {
            TransitionName = transitionName ?? "Pipeline",
            Message = message,
            Severity = severity
        });
    }
}

internal delegate Task<object?> PipelineStep(object? input);

internal sealed class ExceptionGuardPipeline(
    Func<Task> guardTask,
    List<PipelineStep> steps,
    PipelineContext ctx) : IExceptionGuardPipeline
{
    private string _message = "An unexpected error occurred.";

    public IExceptionGuardPipeline WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask)
    {
        var message = _message;

        steps.Add(async _ =>
        {
            if (ctx.Failed || ctx.Notifications.HasErrors)
                return CommandResult<TNext>.Failed(ctx.Notifications.Errors);
            try { await guardTask(); }
            catch { ctx.Fail(message); return default; }
            return await entityTask();
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }
}

internal sealed class GuardWithPipeline(
    Func<Task<bool>> guardTask,
    Func<bool, bool> predicate,
    List<PipelineStep> steps,
    PipelineContext ctx) : IGuardWithPipeline
{
    private string _message = "Guard condition was not satisfied.";

    public IGuardWithPipeline WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask)
    {
        var message = _message;

        steps.Add(async _ =>
        {
            if (ctx.Failed) return default;

            bool result;
            try { result = await guardTask(); }
            catch { ctx.Fail(message); return default; }

            if (!predicate(result)) { ctx.Fail(message); return default; }

            return await entityTask();
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }
}

internal sealed class GuardPipeline(
    Func<Task<bool>> guardTask,
    List<PipelineStep> steps,
    PipelineContext ctx) : IGuardPipeline
{
    public IGuardWithPipeline With(Func<bool, bool> predicate)
        => new GuardWithPipeline(guardTask, predicate, steps, ctx);
}

internal sealed class WithPipeline<T>(
    Func<T, bool> predicate,
    List<PipelineStep> steps,
    PipelineContext ctx) : IWithPipeline<T>
{
    private string _message = $"Validation failed for {typeof(T).Name}.";

    public IWithPipeline<T> WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> nextTask)
    {
        var message = _message;

        steps.Add(async input =>
        {
            if (ctx.Failed) return default;
            var entity = (T?)input;
            if (entity is null) return default;
            if (!predicate(entity)) { ctx.Fail(message); return default; }
            return await nextTask();
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }

    public IHandlerPipeline<T> Invoke(Func<Task> guardTask)
    {
        var message = _message;

        steps.Add(async input =>
        {
            if (ctx.Failed) return default;
            var entity = (T?)input;
            if (entity is null) return default;
            if (!predicate(entity)) { ctx.Fail(message); return default; }
            try { await guardTask(); }
            catch { ctx.Fail("An unexpected error occurred."); return default; }
            return (object?)entity;
        });

        return new HandlerPipeline<T>(steps, ctx);
    }

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<T, Task<TNext?>> nextTask)
    {
        var message = _message;

        steps.Add(async input =>
        {
            if (ctx.Failed) return default;
            var entity = (T?)input;
            if (entity is null) return default;
            if (!predicate(entity)) { ctx.Fail(message); return default; }
            return await nextTask(entity);
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }

    public IHandlerPipeline<T> Invoke(Action<T> transition)
    {
        steps.Add(input =>
        {
            if (ctx.Failed) return Task.FromResult<object?>(input);
            var entity = (T?)input;
            if (entity is null) return Task.FromResult<object?>(null);
            if (!predicate(entity)) { ctx.Fail(_message); return Task.FromResult<object?>(null); }
            transition(entity);
            return Task.FromResult<object?>(entity);
        });

        return new HandlerPipeline<T>(steps, ctx);
    }

    public IHandlerPipeline<T> Save(Func<T, Task> persist)
    {
        var message = _message;

        steps.Add(async input =>
        {
            if (ctx.Failed) return default;
            var entity = (T?)input;
            if (entity is null) return default;
            if (!predicate(entity)) { ctx.Fail(message); return default; }
            await persist(entity);
            return entity;
        });

        return new HandlerPipeline<T>(steps, ctx);
    }

    public Task<ICommandResult<T>> ToResultAsync()
        => new HandlerPipeline<T>(steps, ctx).ToResultAsync();
}

internal sealed class HandlerPipeline<T>(
    List<PipelineStep> steps,
    PipelineContext ctx) : IHandlerPipeline<T>
{
    public IWithPipeline<T> WithCheck(Func<T, bool> predicate)
        => new WithPipeline<T>(predicate, steps, ctx);

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> nextTask)
    {
        steps.Add(async _ =>
        {
            if (ctx.Failed) return default;
            return (object?)await nextTask();
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }

    public IHandlerPipeline<TNext> Invoke<TNext>(Func<T, Task<TNext?>> nextTask)
    {
        steps.Add(async input =>
        {
            if (ctx.Failed) return default;
            var entity = (T?)input;
            if (entity is null) return default;
            return (object?)await nextTask(entity);
        });

        return new HandlerPipeline<TNext>(steps, ctx);
    }

    public IHandlerPipeline<T> Invoke(Action<T> transition)
    {
        steps.Add(input =>
        {
            if (ctx.Failed) return Task.FromResult<object?>(input);
            var entity = (T?)input;
            if (entity is null) return Task.FromResult<object?>(null);
            transition(entity);
            return Task.FromResult<object?>(entity);
        });

        return this;
    }

    public IHandlerPipeline<T> Invoke(Func<Task> guardTask)
    {
        steps.Add(async input =>
        {
            if (ctx.Failed) return default;
            var entity = (T?)input;
            if (entity is null) return default;
            try { await guardTask(); }
            catch { ctx.Fail("An unexpected error occurred."); return default; } // TODO: translate this to a 500 instead of 400
            return (object?)entity;
        });

        return this;
    }

    public IHandlerPipeline<T> Save(Func<T, Task> persist)
    {
        steps.Add(async input =>
        {
            if (ctx.Failed) return default;
            var entity = (T?)input;
            if (entity is null) return default;
            await persist(entity);
            return (object?)entity;
        });

        return this;
    }

    public async Task<ICommandResult<T>> ToResultAsync()
    {
        object? current = null;

        foreach (var step in steps)
        {
            current = await step(current);

            if (ctx.Failed)
                return CommandResult<T>.Failed(ctx.Notifications.Errors);

            if (current is null)
                return CommandResult<T>.Missing();
        }

        return CommandResult<T>.Success((T)current!);
    }
}

public abstract class CommandHandler<TCommand, T>(INotificationBag notifications)
    : IRequestHandler<TCommand, ICommandResult<T>>
    where TCommand : IRequest<ICommandResult<T>>
{
    private readonly PipelineContext _ctx = new(notifications);

    public abstract Task<ICommandResult<T>> Handle(
        TCommand request, CancellationToken cancellationToken);

    protected IExceptionGuardPipeline Invoke(Func<Task> guardTask)
        => new ExceptionGuardPipeline(guardTask, [], _ctx);

    protected IGuardPipeline Invoke(Func<Task<bool>> guardTask)
        => new GuardPipeline(guardTask, [], _ctx);

    protected IHandlerPipeline<T> Invoke(Func<Task<T?>> entityTask)
    {
        var steps = new List<PipelineStep>
        {
            async _ => await entityTask()
        };
        return new HandlerPipeline<T>(steps, _ctx);
    }
}

public interface ICommandResult<T>
{
    T? Entity { get; }
    IReadOnlyList<Notification> Errors { get; }
    bool HasErrors => Errors.Count > 0;
    bool NotFound { get; }
}

public interface IExceptionGuardPipeline
{
    IExceptionGuardPipeline WithMessage(string message);
    IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask);
}

public interface IGuardWithPipeline
{
    IGuardWithPipeline WithMessage(string message);
    IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask);
}

public interface IGuardPipeline
{
    IGuardWithPipeline With(Func<bool, bool> predicate);
}

public interface IWithPipeline<T>
{
    IWithPipeline<T> WithMessage(string message);
    IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask);
    IHandlerPipeline<TNext> Invoke<TNext>(Func<T, Task<TNext?>> entityTask);
    IHandlerPipeline<T> Invoke(Action<T> transition);
    IHandlerPipeline<T> Invoke(Func<Task> guardTask);
    IHandlerPipeline<T> Save(Func<T, Task> persist);
    Task<ICommandResult<T>> ToResultAsync();
}

public interface IHandlerPipeline<T>
{
    IWithPipeline<T> WithCheck(Func<T, bool> predicate);
    IHandlerPipeline<TNext> Invoke<TNext>(Func<Task<TNext?>> entityTask);
    IHandlerPipeline<TNext> Invoke<TNext>(Func<T, Task<TNext?>> entityTask);
    IHandlerPipeline<T> Invoke(Action<T> transition);
    IHandlerPipeline<T> Invoke(Func<Task> guardTask);
    IHandlerPipeline<T> Save(Func<T, Task> persist);
    Task<ICommandResult<T>> ToResultAsync();
}