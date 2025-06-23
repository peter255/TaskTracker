using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence;

namespace TaskTracker.Infrastructure.Repositories;

public class TaskRepository(AppDbContext context) : GenericRepository<TaskItem>(context), ITaskRepository
{
}
