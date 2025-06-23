using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Services;

public interface ITaskGroupService
{
    Task<IEnumerable<TaskGroupDto>> GetAllGroupsAsync();
    Task<TaskGroupDto?> GetGroupByIdAsync(int id);
    Task<bool> AddGroupAsync(CreateTaskGroupDto group);
    Task<bool> UpdateGroupAsync(UpdateTaskGroupDto group);
    Task<bool> DeleteGroupAsync(int id);
}
