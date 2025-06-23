using Microsoft.AspNetCore.Http;
using Moq;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Services;

namespace TaskTracker.Tests;

public class TaskGroupServiceTests
{
    private readonly Mock<ITaskGroupRepository> _mockRepo;
    private readonly ITaskGroupService _taskGroupService;
    private readonly Mock<IHttpContextAccessor> _mckHttpContextAccessor;

    public TaskGroupServiceTests()
    {
        _mockRepo = new Mock<ITaskGroupRepository>();
        _mckHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _taskGroupService = new TaskGroupService(_mockRepo.Object, _mckHttpContextAccessor.Object);
    }

    [Fact]
    public async Task GetAllGroupsAsync_ReturnsAllGroups()
    {
        // Arrange
        var groups = new List<TaskGroup>
        {
            new TaskGroup { Id = 1, Name = "Group 1", UserId = 1 },
            new TaskGroup { Id = 2, Name = "Group 2", UserId = 1 }
        };

        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(groups);

        // Act
        var result = await _taskGroupService.GetAllGroupsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Collection(result,
            item => Assert.Equal("Group 1", item.Name),
            item => Assert.Equal("Group 2", item.Name)
        );
    }

    [Fact]
    public async Task GetAllGroupsAsync_ReturnsEmptyList()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<TaskGroup>());

        // Act
        var result = await _taskGroupService.GetAllGroupsAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGroupByIdAsync_ReturnsGroup_WhenExists()
    {
        // Arrange
        var group = new TaskGroup { Id = 1, Name = "Sample Group", UserId = 1 };
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(group);

        // Act
        var result = await _taskGroupService.GetGroupByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sample Group", result.Name);
    }

    [Fact]
    public async Task GetGroupByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((TaskGroup)null);

        // Act
        var result = await _taskGroupService.GetGroupByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddGroupAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var group = new CreateTaskGroupDto { Name = "New Group" };

        // Act
        await _taskGroupService.AddGroupAsync(group);

        // Assert
        _mockRepo.Verify(x => x.AddAsync(new TaskGroup { Name= group.Name}), Times.Once);
    }

    [Fact]
    public async Task UpdateGroupAsync_CallsRepositoryUpdate()
    {
        // Arrange
        var group = new UpdateTaskGroupDto { Id = 1, Name = "Updated Group" };

        // Act
        await _taskGroupService.UpdateGroupAsync(group);

        // Assert
        _mockRepo.Verify(x => x.Update(new TaskGroup { Name = group.Name }), Times.Once);
    }

    [Fact]
    public async Task DeleteGroupAsync_CallsRepositoryDelete_WhenGroupExists()
    {
        // Arrange
        var group = new TaskGroup { Id = 1, Name = "Group to Delete", UserId = 1 };
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(group);

        // Act
        await _taskGroupService.DeleteGroupAsync(1);

        // Assert
        _mockRepo.Verify(x => x.Delete(group), Times.Once);
    }

    [Fact]
    public async Task DeleteGroupAsync_DoesNotCallRepositoryDelete_WhenGroupNotExists()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((TaskGroup)null);

        // Act
        await _taskGroupService.DeleteGroupAsync(1);

        // Assert
        _mockRepo.Verify(x => x.Delete(It.IsAny<TaskGroup>()), Times.Never);
    }
} 