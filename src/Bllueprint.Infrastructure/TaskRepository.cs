using Bllueprint.Application;
using Bllueprint.Domain;

namespace Bllueprint.Infrastructure;

public class TaskRepository : ITaskRepository
{
    private readonly Dictionary<Guid, TaskItem> _tasks = [];

    public Task<TaskItem?> GetTaskAsync(Guid id) => Task.FromResult(_tasks.TryGetValue(id, out TaskItem? task) ? task : null)!;

    public Task<IEnumerable<TaskItem>> GetAsync() => Task.FromResult(_tasks.Values.AsEnumerable())!;

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _tasks[task.Id] = task;
        await Task.CompletedTask;
        return task;
    }

    public Task SaveAsync(TaskItem task)
    {
        _tasks[task.Id] = task;
        return Task.CompletedTask;
    }
}
