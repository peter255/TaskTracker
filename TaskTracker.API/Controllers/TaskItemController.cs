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
    public TaskItemController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allTasks = await _taskService.GetAllTasksAsync();
        return Ok(allTasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemDto dto)
    {
        return await _taskService.AddTaskAsync(dto) ? Ok(new { message = "Task Created successfully" }) : StatusCode(500);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemDto dto)
    {
        return await _taskService.UpdateTaskAsync(dto) ? Ok(new { message = "Task Updated successfully" }) : StatusCode(500);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _taskService.DeleteTaskAsync(id) ? Ok(new { message = "Task Deleted successfully" }) : StatusCode(500);

    }
}
