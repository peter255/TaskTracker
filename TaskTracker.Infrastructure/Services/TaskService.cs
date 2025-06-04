using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly IGenericRepository<TaskItem> _repository;

    public TaskService(IGenericRepository<TaskItem> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        => await _repository.GetAllAsync();

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task AddTaskAsync(TaskItem task)
        => await _repository.AddAsync(task);

    public async Task UpdateTaskAsync(TaskItem task)
        => _repository.Update(task);

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task != null)
            _repository.Delete(task);
    }
}
