using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Services;

namespace TaskTracker.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<IGenericRepository<TaskItem>> _mockRepo;
        private readonly ITaskService _taskService;

        public TaskServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<TaskItem>>();
            _taskService = new TaskService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsAllTasks()
        {
            //Arrange 
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1" },
                new TaskItem { Id = 2, Title = "Task 2" }
            };

            _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(tasks);

            //Act
            var result = await _taskService.GetAllTasksAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Collection(result,
                item => Assert.Equal("Task 1", item.Title),
                item => Assert.Equal("Task 2", item.Title)
                );
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsEmptyList()
        {
            _mockRepo.Setup(x=> x.GetAllAsync()).ReturnsAsync([]);
           var result= await _taskService.GetAllTasksAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask_WhenExists()
        {

            //Arrange
            var task = new TaskItem { Id = 1, Title = "Sample Task" };

            _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(task);

            //Act
            var resutl = await _taskService.GetTaskByIdAsync(1);

            //Assert
            Assert.NotNull(resutl);
            Assert.Equal("Sample Task", resutl.Title);

        }

        [Fact]
        public async Task AddTaskAsync_CallsRepositoryAddTaskAsync()
        {
            var task = new TaskItem { Id = 1, Title = "Add New" };
            await _taskService.AddTaskAsync(task);
            _mockRepo.Verify(x => x.AddAsync(task), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_CallsRepositoryUpdateTaskAsync()
        {
            var task = new TaskItem { Id = 1, Title = "Update Task Title" };
            await _taskService.UpdateTaskAsync(task);

            _mockRepo.Verify(x => x.Update(task), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_CallsRepositoryDeleteTaskAsync()
        {
            var task = new TaskItem { Id = 1 };
            _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(task);

            await _taskService.DeleteTaskAsync(1);
            _mockRepo.Verify(x => x.Delete(task), Times.Once);
        }
    }
}
