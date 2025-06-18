using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;

namespace TaskTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _tagService.GetAllTagsAsync();

        var result = tags.Select(tag => new TagDto
        {
            Id = tag.Id,
            Name = tag.Name
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        if (tag is null)
            return NotFound();

        return Ok(new TagDto
        {
            Id = tag.Id,
            Name = tag.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTagDto dto)
    {
        var tag = new TaskTracker.Domain.Entities.Tag
        {
            Name = dto.Name
        };

        await _tagService.AddTagAsync(tag);
        return Ok(new { message = "Tag created", tag.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTagDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var existing = await _tagService.GetTagByIdAsync(id);
        if (existing is null)
            return NotFound();

        existing.Name = dto.Name;
        await _tagService.UpdateTagAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        if (tag is null)
            return NotFound();

        await _tagService.DeleteTagAsync(id);
        return NoContent();
    }
}
