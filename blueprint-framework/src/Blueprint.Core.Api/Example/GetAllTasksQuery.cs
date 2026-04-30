using Blueprint.Core.Domain;
using MediatR;

namespace Blueprint.Core.Api.Example;

public record struct GetAllTasksQuery : IRequest<ICommandResult<IEnumerable<TaskItem>>>;

public class GetAllTasksHandler(TaskRepository repository, INotificationContext notifications) : CommandHandler<GetAllTasksQuery, IEnumerable<TaskItem>>(notifications)
{
    public override async Task<ICommandResult<IEnumerable<TaskItem>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        => await Invoke(() => repository.GetAsync()!).ToResultAsync();
}
