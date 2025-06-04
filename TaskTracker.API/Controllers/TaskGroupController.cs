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
public class TaskGroupController : ControllerBase
{
    private readonly ITaskGroupService _service;

    public TaskGroupController(ITaskGroupService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var allGroups = await _service.GetAllGroupsAsync();

        var userGroups = allGroups
            .Where(g => g.UserId == userId)
            .Select(g => new TaskGroupDto
            {
                Id = g.Id,
                Name = g.Name
            });

        return Ok(userGroups);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.GetUserId();
        var group = await _service.GetGroupByIdAsync(id);

        if (group is null || group.UserId != userId)
            return NotFound();

        var dto = new TaskGroupDto
        {
            Id = group.Id,
            Name = group.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskGroupDto dto)
    {
        var userId = User.GetUserId();

        var group = new TaskGroup
        {
            Name = dto.Name,
            UserId = userId
        };

        await _service.AddGroupAsync(group);
        return Ok(new { message = "Group created successfully", group.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskGroupDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var userId = User.GetUserId();
        var existingGroup = await _service.GetGroupByIdAsync(id);

        if (existingGroup is null || existingGroup.UserId != userId)
            return NotFound();

        existingGroup.Name = dto.Name;
        await _service.UpdateGroupAsync(existingGroup);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.GetUserId();
        var group = await _service.GetGroupByIdAsync(id);

        if (group is null || group.UserId != userId)
            return NotFound();

        await _service.DeleteGroupAsync(id);
        return NoContent();
    }
}
