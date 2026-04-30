using Blueprint.Core.Domain;
using MediatR;

namespace Blueprint.Core.Api.Example;

public record struct CreateTaskCommand(string Title) : IRequest<ICommandResult<TaskItem>>;

public class CreateTaskHandler(TaskRepository repository, INotificationContext notifications) : CommandHandler<CreateTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => repository.AddAsync(TaskItem.Create(request.Title))!).ToResultAsync();
}
