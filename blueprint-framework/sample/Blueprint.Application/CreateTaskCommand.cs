using Blueprint.Core.Application;
using Blueprint.Core.Domain;
using Blueprint.Domain;
using MediatR;

namespace Blueprint.Application;

public record struct CreateTaskCommand(string Title) : IRequest<ICommandResult<TaskItem>>;

public class CreateTaskHandler(ITaskRepository repository, INotificationContext notifications) : CommandHandler<CreateTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => repository.AddAsync(TaskItem.Create(request.Title))!).ToResultAsync();
}
