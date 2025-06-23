using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskItemDto>> GetAllTasksAsync();
    Task<TaskItemDto?> GetTaskByIdAsync(int id);
    Task<bool> AddTaskAsync(CreateTaskItemDto task);
    Task<bool> UpdateTaskAsync(UpdateTaskItemDto task);
    Task<bool> DeleteTaskAsync(int id);
}
