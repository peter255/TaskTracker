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

        var allGroups = await _service.GetAllGroupsAsync();
        return Ok(allGroups);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    { 
        var group = await _service.GetGroupByIdAsync(id);

        if (group is null)
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
      return  await _service.AddGroupAsync(dto) ? Ok(new { message = "Group created successfully" }) : StatusCode(500);
          
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateTaskGroupDto dto)
    {
       return await _service.UpdateGroupAsync(dto) ? Ok(new { message = "Group created successfully" }) : StatusCode(500);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      return  await _service.DeleteGroupAsync(id) ? Ok(new { message = "Group created successfully" }) : StatusCode(500);
    }
}
