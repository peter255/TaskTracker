
using TaskTracker.Domain.Entities;
using  TaskTracker.Infrastructure.Persistence;
using HotChocolate; 
using HotChocolate.Types;
using HotChocolate.Data;

namespace TaskTracker.Infrastructure.GraphQL;


public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<TaskItem> GetTasks([Service] AppDbContext db)
        => db.TaskItems;
}