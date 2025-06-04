using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Services;

public interface ITaskGroupService
{
    Task<IEnumerable<TaskGroup>> GetAllGroupsAsync();
    Task<TaskGroup?> GetGroupByIdAsync(int id);
    Task AddGroupAsync(TaskGroup group);
    Task UpdateGroupAsync(TaskGroup group);
    Task DeleteGroupAsync(int id);
}
