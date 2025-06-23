using Microsoft.AspNetCore.Http;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Extensions;

namespace TaskTracker.Infrastructure.Services;

public class TaskGroupService : ITaskGroupService
{
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly IHttpContextAccessor _accessor;

    public TaskGroupService(ITaskGroupRepository taskGroupRepository, IHttpContextAccessor accessor)
    {
        _taskGroupRepository = taskGroupRepository;
        _accessor = accessor;
    }

    public async Task<IEnumerable<TaskGroupDto>> GetAllGroupsAsync()
    {
        return await _taskGroupRepository.GetSpecificAsync(
            filter: x => x.UserId == GetUserId(),
            select: x => new TaskGroupDto
            {
                Id = x.Id,
                Name = x.Name
            });
    }

    public async Task<TaskGroupDto?> GetGroupByIdAsync(int id)
    {
        TaskGroup? taskGroup = await _taskGroupRepository.GetFirstOrDefaultAsync(filter: x => x.UserId == GetUserId() && x.Id == id);
        if (taskGroup != null)
        {
            return new TaskGroupDto
            {
                Id = taskGroup.Id,
                Name = taskGroup.Name
            };
        }

        return null;
    }

    public async Task<bool> AddGroupAsync(CreateTaskGroupDto group)
    {
        try
        {
            await _taskGroupRepository.AddAsync(new TaskGroup { Name = group.Name });
            return await _taskGroupRepository.SaveChangesAsync();
        }
        catch
        {
            return false; // Consider logging the exception or handling it as needed
        }
    }

    public async Task<bool> UpdateGroupAsync(UpdateTaskGroupDto group)
    {
        try
        {
            _taskGroupRepository.Update(new TaskGroup { Name = group.Name });
            return await _taskGroupRepository.SaveChangesAsync();
        }
        catch
        {
            return false; // Consider logging the exception or handling it as needed
        }
    }

    public async Task<bool> DeleteGroupAsync(int id)
    {
        try
        {


            var group = await _taskGroupRepository.GetByIdAsync(id);
            if (group != null)
            {
                _taskGroupRepository.Delete(group);
                return await _taskGroupRepository.SaveChangesAsync();
            }
        }
        catch (Exception ex)
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