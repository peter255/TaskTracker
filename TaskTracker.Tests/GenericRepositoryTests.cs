using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence;
using TaskTracker.Infrastructure.Repositories;

namespace TaskTracker.Tests;

public class GenericRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly IGenericRepository<TaskItem> _repository;

    public GenericRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new AppDbContext(options);
        _repository = new GenericRepository<TaskItem>(_context);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem { Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(1) },
            new TaskItem { Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(2) }
        };
        await _context.TaskItems.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, t => t.Title == "Task 1");
        Assert.Contains(result, t => t.Title == "Task 2");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEntity_WhenExists()
    {
        // Arrange
        var task = new TaskItem { Title = "Test Task", Description = "Test Description", DueDate = DateTime.Now.AddDays(1) };
        await _context.TaskItems.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(task.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(task.Title, result.Title);
        Assert.Equal(task.Description, result.Description);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsEntityToDatabase()
    {
        // Arrange
        var task = new TaskItem { Title = "New Task", Description = "New Description", DueDate = DateTime.Now.AddDays(1) };

        // Act
        await _repository.AddAsync(task);
        await _context.SaveChangesAsync();

        // Assert
        var savedTask = await _context.TaskItems.FindAsync(task.Id);
        Assert.NotNull(savedTask);
        Assert.Equal(task.Title, savedTask.Title);
        Assert.Equal(task.Description, savedTask.Description);
    }

    [Fact]
    public void Update_ModifiesEntityInDatabase()
    {
        // Arrange
        var task = new TaskItem { Title = "Original Title", Description = "Original Description", DueDate = DateTime.Now.AddDays(1) };
        _context.TaskItems.Add(task);
        _context.SaveChanges();

        // Act
        task.Title = "Updated Title";
        task.Description = "Updated Description";
        _repository.Update(task);
        _context.SaveChanges();

        // Assert
        var updatedTask = _context.TaskItems.Find(task.Id);
        Assert.NotNull(updatedTask);
        Assert.Equal("Updated Title", updatedTask.Title);
        Assert.Equal("Updated Description", updatedTask.Description);
    }

    [Fact]
    public void Delete_RemovesEntityFromDatabase()
    {
        // Arrange
        var task = new TaskItem { Title = "Task to Delete", Description = "Description", DueDate = DateTime.Now.AddDays(1) };
        _context.TaskItems.Add(task);
        _context.SaveChanges();

        // Act
        _repository.Delete(task);
        _context.SaveChanges();

        // Assert
        var deletedTask = _context.TaskItems.Find(task.Id);
        Assert.Null(deletedTask);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoEntities()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }
} 