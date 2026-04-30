using Blueprint.Core.Domain;
using MediatR;

namespace Blueprint.Core.Api.Example;

public record struct StartTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;

public class StartTaskHandler(TaskRepository repository, INotificationContext notifications)
    : CommandHandler<StartTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(StartTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => repository.GetTaskAsync(request.Id))
                    .Invoke(task => task.Start())
                    .Save(repository.SaveAsync).ToResultAsync();
}
