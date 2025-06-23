using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Infrastructure.Persistence;

namespace TaskTracker.Infrastructure.Repositories;

public class TagRepository(AppDbContext context) : GenericRepository<Domain.Entities.Tag>(context), ITagRepository
{

}
