using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Services;

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
        return await _tagService.AddTagAsync(dto) ? Ok(new { message = "Tag created" }) : StatusCode(500);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTagDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        return await _tagService.UpdateTagAsync(dto) ? Ok(new { message = "Tag Updated" }) : StatusCode(500);


    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        if (tag is null)
            return NotFound();

      return  await _tagService.DeleteTagAsync(id) ? Ok(new { message = "Tag Deleted" }) : StatusCode(500);

    }
}
