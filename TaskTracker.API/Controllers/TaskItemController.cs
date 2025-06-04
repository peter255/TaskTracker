using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.API.Extensions;

namespace TaskTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ITaskGroupService _groupService;

    public TaskItemController(ITaskService taskService, ITaskGroupService groupService)
    {
        _taskService = taskService;
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var allTasks = await _taskService.GetAllTasksAsync();

        var result = allTasks
            .Where(t => t.TaskGroup?.UserId == userId)
            .Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                TaskGroupId = t.TaskGroupId,
                Status = t.Status.ToString()
            });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.GetUserId();
        var task = await _taskService.GetTaskByIdAsync(id);

        if (task == null || task.TaskGroup?.UserId != userId)
            return NotFound();

        var dto = new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            TaskGroupId = task.TaskGroupId,
            Status = task.Status.ToString()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemDto dto)
    {
        var userId = User.GetUserId();
        var group = await _groupService.GetGroupByIdAsync(dto.TaskGroupId);

        if (group == null || group.UserId != userId)
            return BadRequest("Invalid group or unauthorized");

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            TaskGroupId = dto.TaskGroupId
        };

        await _taskService.AddTaskAsync(task);
        return Ok(new { message = "Task created successfully", task.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var userId = User.GetUserId();
        var existingTask = await _taskService.GetTaskByIdAsync(id);

        if (existingTask == null || existingTask.TaskGroup?.UserId != userId)
            return NotFound();

        existingTask.Title = dto.Title;
        existingTask.Description = dto.Description;
        existingTask.DueDate = dto.DueDate;
        existingTask.Status = Enum.TryParse(dto.Status, out Domain.Enums.TaskStatus status) ? status : Domain.Enums.TaskStatus.Pending;

        await _taskService.UpdateTaskAsync(existingTask);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.GetUserId();
        var task = await _taskService.GetTaskByIdAsync(id);

        if (task == null || task.TaskGroup?.UserId != userId)
            return NotFound();

        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }
}
