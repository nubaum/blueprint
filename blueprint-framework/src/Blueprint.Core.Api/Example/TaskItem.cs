using Blueprint.Core.Domain;

namespace Blueprint.Core.Api.Example;

public class TaskItem : Aggregate<TaskItem>
{
    private static readonly Transition<TaskItem> _complete = TransitionBuilder
        .Do(o => o.Status = TaskStatus.Done)
        .Requires(o => o.Status == TaskStatus.InProgress, "A task can only be completed when in progress.")
        .Create();

    private static readonly Transition<TaskItem> _start = TransitionBuilder
        .Do(o => o.Status = TaskStatus.InProgress)
        .Requires(o => o.Status == TaskStatus.ToDo, "A task can only start if it's current state is InProgress")
        .Create();

    private static readonly Transition<TaskItem> _cancel = TransitionBuilder
        .Do(o => o.Status = TaskStatus.Cancelled)
        .Requires(o => o.Status is TaskStatus.InProgress or TaskStatus.ToDo, "A complete task can't be canceled")
        .Create();

    private static readonly Transition<TaskItem> _reopen = TransitionBuilder
        .Do(o => o.Status = TaskStatus.ToDo)
        .Requires(o => o.Status is TaskStatus.Cancelled or TaskStatus.Done, "A task can only be reopend when it's cancelled or completed")
        .Create();

    private static readonly Transition<TaskItem, string> _rename = CreateTransitionWith<string>()
        .Do((o, newName) => o.Title = newName)
        .Requires((o, _) => o.Status is TaskStatus.ToDo or TaskStatus.InProgress, "A task can't be renamed if it's complete or cancelled")
        .Create();

    private TaskItem()
    {
    }

    public Guid Id { get; private set; }

    public string? Title { get; private set; }

    public TaskStatus Status { get; private set; }

    public DateTimeOffset? StartedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public static TaskItem Create(string title) =>
        new()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Status = TaskStatus.ToDo
        };

    public void Start() => _start.Invoke(this);

    public void Complete() => _complete.Invoke(this);

    public void Cancel() => _cancel.Invoke(this);

    public void Reopen() => _reopen.Invoke(this);

    public void Rename(string newName) => _rename.Invoke(this, newName);
}
