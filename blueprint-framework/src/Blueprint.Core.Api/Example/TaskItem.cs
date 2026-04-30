using Blueprint.Core.Domain;

namespace Blueprint.Core.Api.Example;

public class TaskItem : Aggregate
{
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

    public void Start()
    {
        if (Status == TaskStatus.ToDo)
        {
            Status = TaskStatus.InProgress;
        }
        else
        {
            AddViolationMessage("A task can only start if it's current state is InProgress");
        }
    }

    public void Complete()
    {
        if (Status == TaskStatus.InProgress)
        {
            Status = TaskStatus.Done;
        }
        else
        {
            AddViolationMessage("A task can only be complete when It's in progress");
        }
    }

    public void Cancel()
    {
        if (Status is TaskStatus.InProgress or TaskStatus.ToDo)
        {
            Status = TaskStatus.Cancelled;
        }
        else
        {
            AddViolationMessage("A complete task can't be canceled");
        }
    }

    public void Reopen()
    {
        if (Status is TaskStatus.Cancelled or TaskStatus.Done)
        {
            Status = TaskStatus.ToDo;
        }
        else
        {
            AddViolationMessage("A task can only be reopend when it's cancelled or completed");
        }
    }

    public void Rename(string newName)
    {
        if(Status == TaskStatus.Done || Status == TaskStatus.Cancelled)
        {
            AddViolationMessage("A task can't be renamed if it's complete or cancelled");
        }
        else
        {
            Title = newName;
        }
    }
}
