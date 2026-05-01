using Blueprint.Core.Application;
using Blueprint.Core.Domain;
using Blueprint.Domain;
using MediatR;

namespace Blueprint.Application;

public record struct StartTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;

public class StartTaskHandler(ITaskRepository repository, INotificationContext notifications)
    : CommandHandler<StartTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(StartTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => repository.GetTaskAsync(request.Id))
                    .Invoke(task => task.Start())
                    .Save(repository.SaveAsync).ToResultAsync();
}
