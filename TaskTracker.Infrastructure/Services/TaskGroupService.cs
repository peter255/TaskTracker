using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Services;

public class TaskGroupService : ITaskGroupService
{
    private readonly IGenericRepository<TaskGroup> _repository;

    public TaskGroupService(IGenericRepository<TaskGroup> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskGroup>> GetAllGroupsAsync()
        => await _repository.GetAllAsync();

    public async Task<TaskGroup?> GetGroupByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task AddGroupAsync(TaskGroup group)
        => await _repository.AddAsync(group);

    public async Task UpdateGroupAsync(TaskGroup group)
        => _repository.Update(group);

    public async Task DeleteGroupAsync(int id)
    {
        var group = await _repository.GetByIdAsync(id);
        if (group != null)
            _repository.Delete(group);
    }
}
