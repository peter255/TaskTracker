using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly IGenericRepository<TaskTracker.Domain.Entities.Tag> _repository;

    public TagService(IGenericRepository<TaskTracker.Domain.Entities.Tag> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskTracker.Domain.Entities.Tag>> GetAllTagsAsync()
        => await _repository.GetAllAsync();

    public async Task<TaskTracker.Domain.Entities.Tag?> GetTagByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task AddTagAsync(TaskTracker.Domain.Entities.Tag tag)
        => await _repository.AddAsync(tag);

    public async Task UpdateTagAsync(TaskTracker.Domain.Entities.Tag tag)
        => _repository.Update(tag);

    public async Task DeleteTagAsync(int id)
    {
        var tag = await _repository.GetByIdAsync(id);
        if (tag != null)
            _repository.Delete(tag);
    }
}

