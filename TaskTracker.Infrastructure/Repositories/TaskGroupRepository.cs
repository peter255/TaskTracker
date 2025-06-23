using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence;

namespace TaskTracker.Infrastructure.Repositories;

public class TaskGroupRepository(AppDbContext context) : GenericRepository<TaskGroup>(context), ITaskGroupRepository
{

}
