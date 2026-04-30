using Blueprint.Core.Domain;
using MediatR;

namespace Blueprint.Core.Api.Example;

public record struct ReopenTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;

public class ReopenTaskHandler(TaskRepository repository, INotificationContext notifications)
    : CommandHandler<ReopenTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(ReopenTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => repository.GetTaskAsync(request.Id))
                    .Invoke(task => task.Reopen())
                    .Save(repository.SaveAsync).ToResultAsync();
}
