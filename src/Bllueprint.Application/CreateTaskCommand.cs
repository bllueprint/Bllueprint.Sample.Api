using Bllueprint.Core.Application;
using Bllueprint.Core.Domain;
using Bllueprint.Domain;
using MediatR;

namespace Bllueprint.Application;

public record struct CreateTaskCommand(string Title) : IRequest<ICommandResult<TaskItem>>;

public class CreateTaskHandler(ITaskRepository repository, INotificationContext notifications) : CommandHandler<CreateTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => repository.AddAsync(new TaskItem(request.Title))!).ToResultAsync();
}
