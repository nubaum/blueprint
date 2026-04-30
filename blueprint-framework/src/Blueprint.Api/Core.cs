using Blueprint.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blueprint.Api;


public interface ICommandResult<T>
{
    T? Entity { get; }
    IReadOnlyList<Notification> Errors { get; }
    bool HasErrors => Errors.Count > 0;
    bool NotFound { get; }
}

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


public abstract class CommandHandler<TCommand, T>(INotificationBag notifications)
    : IRequestHandler<TCommand, ICommandResult<T>>
    where TCommand : IRequest<ICommandResult<T>>
{
    public abstract Task<ICommandResult<T>> Handle(
        TCommand request, CancellationToken cancellationToken);

    protected IHandlerPipeline<T> Invoke(Task<T?> entityTask)
        => new HandlerPipeline<T>(entityTask, notifications);
}

public interface IHandlerPipeline<T>
{
    IHandlerPipeline<T> Change(Action<T> transition);

    IHandlerPipeline<T> Save(Func<T, Task> persist);

    Task<ICommandResult<T>> ToResultAsync();
}

internal sealed class HandlerPipeline<T> : IHandlerPipeline<T>
{
    private readonly Task<T?> _entityTask;
    private readonly INotificationBag _notifications;
    private Action<T>? _transition;
    private Func<T, Task>? _persist;

    internal HandlerPipeline(Task<T?> entityTask, INotificationBag notifications)
    {
        _entityTask = entityTask;
        _notifications = notifications;
    }

    public IHandlerPipeline<T> Change(Action<T> transition)
    {
        _transition = transition;
        return this;
    }

    public IHandlerPipeline<T> Save(Func<T, Task> persist)
    {
        _persist = persist;
        return this;
    }

    public async Task<ICommandResult<T>> ToResultAsync()
    {
        var entity = await _entityTask;

        if (entity is null)
            return CommandResult<T>.Missing();

        _transition?.Invoke(entity);

        if (_notifications.HasErrors)
            return CommandResult<T>.Failed(_notifications.Errors);

        if (_persist is not null)
            await _persist(entity);

        return CommandResult<T>.Success(entity);
    }
}