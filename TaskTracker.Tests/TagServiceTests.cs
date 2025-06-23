using Microsoft.AspNetCore.Http;
using Moq;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Services;

namespace TaskTracker.Tests;

public class TagServiceTests
{
    private readonly Mock<ITagRepository> _mockRepo;
    private readonly ITagService _tagService;
    private readonly Mock<IHttpContextAccessor> _mckHttpContextAccessor;

    public TagServiceTests()
    {
        _mockRepo = new Mock<ITagRepository>();
        _mckHttpContextAccessor= new Mock<IHttpContextAccessor>();
        _tagService = new TagService(_mockRepo.Object, _mckHttpContextAccessor.Object);
    }

    [Fact]
    public async Task GetAllTagsAsync_ReturnsAllTags()
    {
        // Arrange
        var tags = new List<Tag>
        {
            new Tag { Id = 1, Name = "Tag 1" },
            new Tag { Id = 2, Name = "Tag 2" }
        };

        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(tags);

        // Act
        var result = await _tagService.GetAllTagsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Collection(result,
            item => Assert.Equal("Tag 1", item.Name),
            item => Assert.Equal("Tag 2", item.Name)
        );
    }

    [Fact]
    public async Task GetAllTagsAsync_ReturnsEmptyList()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Tag>());

        // Act
        var result = await _tagService.GetAllTagsAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTagByIdAsync_ReturnsTag_WhenExists()
    {
        // Arrange
        var tag = new Tag { Id = 1, Name = "Sample Tag" };
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(tag);

        // Act
        var result = await _tagService.GetTagByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sample Tag", result.Name);
    }

    [Fact]
    public async Task GetTagByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Tag)null);

        // Act
        var result = await _tagService.GetTagByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddTagAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var tag = new CreateTagDto { Name = "New Tag" };


        // Act
        await _tagService.AddTagAsync(tag);

        // Assert
        _mockRepo.Verify(x => x.AddAsync(new Tag { Name = tag.Name }), Times.Once);
    }

    [Fact]
    public async Task UpdateTagAsync_CallsRepositoryUpdate()
    {
        // Arrange
        var tag = new UpdateTagDto { Id = 1, Name = "Updated Tag" };

        // Act
        await _tagService.UpdateTagAsync(tag);

        // Assert
        _mockRepo.Verify(x => x.Update(new Tag { Id = tag.Id, Name = tag.Name }), Times.Once);
    }

    [Fact]
    public async Task DeleteTagAsync_CallsRepositoryDelete_WhenTagExists()
    {
        // Arrange
        var tag = new Tag { Id = 1, Name = "Tag to Delete" };
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(tag);

        // Act
        await _tagService.DeleteTagAsync(1);

        // Assert
        _mockRepo.Verify(x => x.Delete(tag), Times.Once);
    }

    [Fact]
    public async Task DeleteTagAsync_DoesNotCallRepositoryDelete_WhenTagNotExists()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Tag)null);

        // Act
        await _tagService.DeleteTagAsync(1);

        // Assert
        _mockRepo.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
    }
}