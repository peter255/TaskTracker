using Microsoft.AspNetCore.Http;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Extensions;

namespace TaskTracker.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IHttpContextAccessor _accessor;

    public TaskService(ITaskRepository taskRepository, IHttpContextAccessor accessor)
    {
        _taskRepository = taskRepository;
        _accessor = accessor;
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllTasksAsync()
    {
        return await _taskRepository.GetSpecificAsync(
             filter: x => x.TaskGroup!.UserId == GetUserId(),
             select: x => new TaskItemDto
             {
                 Id = x.Id,
                 Title = x.Title,
                 Description = x.Description,
                 DueDate = x.DueDate,
                 Status = x.Status.ToString(),
                 TaskGroupId = x.TaskGroupId
             });
    }
    public async Task<TaskItemDto?> GetTaskByIdAsync(int id)
    {
        TaskItem? task = await _taskRepository.GetFirstOrDefaultAsync(filter: x => x.TaskGroup!.UserId == GetUserId());

        if (task != null)
        {
            return new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                TaskGroupId = task.TaskGroupId,
                Status = task.Status.ToString()
            };
        }
        return null;
    }
    public async Task<bool> AddTaskAsync(CreateTaskItemDto task)
    {
        try
        {
            await _taskRepository.AddAsync(new TaskItem
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                TaskGroupId = task.TaskGroupId
            });

            return await _taskRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false; // Log the exception as needed
        }
    }

    public async Task<bool> UpdateTaskAsync(UpdateTaskItemDto task)
    {
        try
        {


            _taskRepository.Update(new TaskItem
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                TaskGroupId = task.TaskGroupId
            });

            return await _taskRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false; // Log the exception as needed
        }
    }
    public async Task<bool> DeleteTaskAsync(int id)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                _taskRepository.Delete(task);
                return await _taskRepository.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            // Log the exception as needed
        }
        return false;
    }


    private int GetUserId()
    {
        return _accessor.HttpContext.User.GetUserId();
    }
}
