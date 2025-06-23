using Microsoft.AspNetCore.Http;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Extensions;

namespace TaskTracker.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IHttpContextAccessor _accessor;

    public TagService(ITagRepository tagRepository, IHttpContextAccessor accessor)
    {
        _tagRepository = tagRepository;
        _accessor = accessor;
    }

    public async Task<IEnumerable<TagDetailsDto>> GetAllTagsAsync()
    {
        return await _tagRepository.GetSpecificAsync(filter: x => x.TaskTags.Any(a => a.TaskItem!.TaskGroup!.UserId == GetUserId()), select: x => new TagDetailsDto
        {
            Id = x.Id,
            Name = x.Name
        });
    }
    public async Task<TagDetailsDto?> GetTagByIdAsync(int id)
    {
        var tag = await _tagRepository.GetFirstOrDefaultAsync(filter: x => x.TaskTags.Any(a => a.TaskItem!.TaskGroup!.UserId == GetUserId() && x.Id == id));

        if (tag != null)
        {
            return new TagDetailsDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }
        return null;
    }
    public async Task<bool> AddTagAsync(CreateTagDto tag)
    {
        try
        {
            await _tagRepository.AddAsync(new Tag { Name = tag.Name });
            return await _tagRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false; // Consider logging the exception or handling it as needed
        }
    }
    public async Task<bool> UpdateTagAsync(UpdateTagDto tag)
    {
        try
        {
            _tagRepository.Update(new Tag { Name = tag.Name });
            return await _tagRepository.SaveChangesAsync();
        }
        catch (Exception)
        {

            return false; // Consider logging the exception or handling it as needed
        }
    }
    public async Task<bool> DeleteTagAsync(int id)
    {
        try
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag != null)
            {
                _tagRepository.Delete(tag);
                return await _tagRepository.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            return false; // Consider logging the exception or handling it as needed
        }
        return false;
    }

    private int GetUserId()
    {
        return _accessor.HttpContext.User.GetUserId();
    }
}

