using Bllueprint.Domain;

namespace Bllueprint.Application;

public interface ITaskRepository
{
    Task<TaskItem> AddAsync(TaskItem task);

    Task<IEnumerable<TaskItem>> GetAsync();

    Task<TaskItem?> GetTaskAsync(Guid id);

    Task SaveAsync(TaskItem task);
}
