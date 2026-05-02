using Bllueprint.Core.Application;
using Bllueprint.Core.Domain;
using Bllueprint.Domain;
using MediatR;

namespace Bllueprint.Application;

public record struct CompleteTaskCommand(Guid Id) : IRequest<ICommandResult<TaskItem>>;

public class CompleteTaskHandler(ITaskRepository repository, INotificationContext notifications, IAccessValidator client)
    : CommandHandler<CompleteTaskCommand, TaskItem>(notifications)
{
    public override async Task<ICommandResult<TaskItem>> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        => await Invoke(() => client.ValidateAccessAsync(Guid.NewGuid()))
                    //// WithMessage("Access denied to complete task") // this overrides inner exception message but not a previously set message to the current notification context.
                    .Invoke(() => repository.GetTaskAsync(request.Id))
                        //// .WithCheck(o => o.Status == TaskStatus.Cancelled)
                        //// .WithMessage("This is working")
                    .Invoke(t => t.Complete())
                    //// .Invoke(() => client.ValidateAccessAsync(Guid.Empty))
                    .Save(repository.SaveAsync)
                    .ToResultAsync();
}
