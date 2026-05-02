using Bllueprint.Core.Application;
using Bllueprint.Core.Domain;
using Bllueprint.Domain;
using MediatR;

namespace Bllueprint.Application;

public record struct GetAllTasksQuery : IRequest<ICommandResult<IEnumerable<TaskItem>>>;

public class GetAllTasksHandler(ITaskRepository repository, INotificationContext notifications) : CommandHandler<GetAllTasksQuery, IEnumerable<TaskItem>>(notifications)
{
    public override async Task<ICommandResult<IEnumerable<TaskItem>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        => await Invoke(() => repository.GetAsync()!).ToResultAsync();
}
