using Blueprint.Core.Application;
using Blueprint.Core.Domain;
using Blueprint.Domain;
using MediatR;

namespace Blueprint.Application;

public record struct GetAllTasksQuery : IRequest<ICommandResult<IEnumerable<TaskItem>>>;

public class GetAllTasksHandler(ITaskRepository repository, INotificationContext notifications) : CommandHandler<GetAllTasksQuery, IEnumerable<TaskItem>>(notifications)
{
    public override async Task<ICommandResult<IEnumerable<TaskItem>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        => await Invoke(() => repository.GetAsync()!).ToResultAsync();
}
