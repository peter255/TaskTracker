using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Services;

public interface ITagService
{
    Task<IEnumerable<TagDetailsDto>> GetAllTagsAsync();
    Task<TagDetailsDto?> GetTagByIdAsync(int id);
    Task<bool> AddTagAsync(CreateTagDto tag);
    Task<bool> UpdateTagAsync(UpdateTagDto tag);
    Task<bool> DeleteTagAsync(int id);
}