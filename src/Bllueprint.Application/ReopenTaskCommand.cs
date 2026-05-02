using Bllueprint.Core.Application;
using Bllueprint.Core.Domain;
using Bllueprint.Domain;
using MediatR;

namespace Bllueprint.Application;

public record struct ReopenTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;

public class ReopenTaskHandler(ITaskRepository repository, INotificationContext notifications)
    : CommandHandler<ReopenTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(ReopenTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => repository.GetTaskAsync(request.Id))
                    .Invoke(task => task.Reopen())
                    .Save(repository.SaveAsync).ToResultAsync();
}
