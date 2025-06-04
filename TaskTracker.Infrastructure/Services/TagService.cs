using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly IGenericRepository<Tag> _repository;

    public TagService(IGenericRepository<Tag> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        => await _repository.GetAllAsync();

    public async Task<Tag?> GetTagByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task AddTagAsync(Tag tag)
        => await _repository.AddAsync(tag);

    public async Task UpdateTagAsync(Tag tag)
        => _repository.Update(tag);

    public async Task DeleteTagAsync(int id)
    {
        var tag = await _repository.GetByIdAsync(id);
        if (tag != null)
            _repository.Delete(tag);
    }
}

