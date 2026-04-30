using System.Linq.Expressions;
using Blueprint.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blueprint.Api;

public class TaskRepository
{
    private readonly Dictionary<Guid, TaskItem> _tasks = [];

    public Task<TaskItem?> GetTaskAsync(Guid id) => Task.FromResult(_tasks.TryGetValue(id, out var task) ? task : null)!;

    public Task<IEnumerable<TaskItem>> GetAsync() => Task.FromResult(_tasks.Values.AsEnumerable())!;

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _tasks[task.Id] = task;  
        await Task.CompletedTask;
        return task;
    } 

    public Task SaveAsync(TaskItem task)
    {
        _tasks[task.Id] = task;
        return Task.CompletedTask;
    }
}
public enum TaskStatus { ToDo, InProgress, Done, Cancelled }


[ApiController]
[Route("api/[controller]")]
public class TaskController() : AppController
{
    [HttpGet()]
    public async Task<IActionResult> GetAllAsync() => await SendAsync(new GetAllTasksQuery());

    [HttpPost()]
    public async Task<IActionResult> CreateAsync([FromQuery] string title) => await SendAsync(new CreateTaskCommand(title));

    [HttpPut("{id}/start")]
    public async Task<IActionResult> StartAsync(Guid id) => await SendAsync(new StartTaskCommand(id));

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteAsync(Guid id) => await SendAsync(new CompleteTaskCommand(id));

    [HttpPut("{id}/reopen")]
    public async Task<IActionResult> ReopenAsync(Guid id) => await SendAsync(new ReopenTaskCommand(id));
}
public class TaskItem : Aggregate
{

    private TaskItem()
    {
    }


    public static TaskItem Create(string title) =>
        new() { Id = Guid.NewGuid(), Title = title, Status = TaskStatus.ToDo };

    public Guid Id { get; private set; }
    public string? Title { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTimeOffset? StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public void Start() => ExecuteTransition(_start);
    public void Complete() => ExecuteTransition(_complete);
    public void Cancel() => ExecuteTransition(_cancel);
    public void Reopen() => ExecuteTransition(_reopen);

    private static readonly ITransitionRule<TaskItem> _start =
        Transition<TaskItem>.Named(nameof(Start))
            .Set(t => t.Status, TaskStatus.InProgress)
            .Set(t => t.StartedAt, () => DateTimeOffset.UtcNow)
            .Requires(t => t.Status == TaskStatus.ToDo)
            .WithErrorMessage("A task in progress or complete can't be set in progress")
            .Build();

    private static readonly ITransitionRule<TaskItem> _complete =
        Transition<TaskItem>.Named(nameof(Complete))
            .Set(t => t.Status, TaskStatus.Done)
            .Set(t => t.CompletedAt, () => DateTimeOffset.UtcNow)
            .Requires(t => t.Status == TaskStatus.InProgress)
            .WithErrorMessage("Only in-progress tasks can be completed")
            .Build();

    private static readonly ITransitionRule<TaskItem> _cancel =
        Transition<TaskItem>.Named(nameof(Cancel))
            .Set(t => t.Status, TaskStatus.Cancelled)
            .Requires(t => t.Status != TaskStatus.Done)
            .Requires(t => t.Status != TaskStatus.Cancelled)
            .WithErrorMessage("Done or already-cancelled tasks cannot be cancelled",
                              NotificationSeverity.Warning)
            .Build();

    private static readonly ITransitionRule<TaskItem> _reopen =
        Transition<TaskItem>.Named(nameof(Reopen))
            .Set(t => t.Status, TaskStatus.ToDo)
            .Set(t => t.StartedAt, (DateTimeOffset?)null)
            .Set(t => t.CompletedAt, (DateTimeOffset?)null)
            .Requires(IsComplete)
            .WithErrorMessage("Only done or cancelled tasks can be reopened")
            .Build();

    private static Expression<Func<TaskItem, bool>> IsComplete => t => t.Status == TaskStatus.Cancelled || t.Status == TaskStatus.Done;
}

public record struct GetAllTasksQuery : IRequest<ICommandResult<IEnumerable<TaskItem>>>;
public record struct CreateTaskCommand(string Title) : IRequest<ICommandResult<TaskItem>>;
public record struct StartTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;
public record struct CompleteTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;
public record struct ReopenTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;

public class GetAllTasksHandler(TaskRepository repository, INotificationBag notifications) : CommandHandler<GetAllTasksQuery, IEnumerable<TaskItem>>(notifications)
{
    public override async Task<ICommandResult<IEnumerable<TaskItem>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        => await Invoke(repository.GetAsync()!).ToResultAsync();

}

public class CreateTaskHandler(TaskRepository repository, INotificationBag notifications) : CommandHandler<CreateTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(repository.AddAsync(TaskItem.Create(request.Title))!).ToResultAsync();
}

public class StartTaskHandler(TaskRepository repository, INotificationBag notifications)
    : CommandHandler<StartTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(StartTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(repository.GetTaskAsync(request.Id))
                    .Change(task => task.Start())
                    .Save(repository.SaveAsync).ToResultAsync();
}

public class ReopenTaskHandler(TaskRepository repository, INotificationBag notifications)
    : CommandHandler<ReopenTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(ReopenTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(repository.GetTaskAsync(request.Id))
                    .Change(task => task.Reopen())
                    .Save(repository.SaveAsync).ToResultAsync();
}

public class CompleteTaskHandler(TaskRepository repository, INotificationBag notifications)
    : CommandHandler<CompleteTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(repository.GetTaskAsync(request.Id))
                    .Change(task => task.Complete())
                    .Save(repository.SaveAsync)
                    .ToResultAsync();
}
