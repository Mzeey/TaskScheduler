using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mzeey.Entities;
using Mzeey.TaskSchedulerLib.Repositories;
using Mzeey.TaskSchedulerLib.Services;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Extensions;
using Mzeey.SharedLib.Utilities;

namespace TaskScheduler.Tests
{
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _mockTaskRepository;
        private TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _taskService = new TaskService(_mockTaskRepository.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ValidInput_ReturnsCreatedTask()
        {
            // Arrange
            string title = "Test Task";
            string description = "This is a test task";
            string userId = "user1";
            DateTime? dueDate = null;

            TaskItem createdTask = new TaskItem
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                Title = title,
                Description = description,
                UserId = userId.ToUpper(),
                DueDate = dueDate,
                Status = TaskItemStatus.Pending.GetDescription()
            };

            _mockTaskRepository.Setup(repo => repo.CreateAsync(It.IsAny<TaskItem>()))
                .ReturnsAsync(createdTask);

            // Act
            TaskItem result = await _taskService.CreateTaskAsync(title, description, userId, dueDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(title, result.Title);
            Assert.Equal(description, result.Description);
            Assert.Equal(userId.ToUpper(), result.UserId);
            Assert.Equal(TaskItemStatus.Pending.GetDescription(), result.Status);
        }

        [Fact]
        public async Task EditTaskAsync_ValidInput_ReturnsUpdatedTask()
        {
            // Arrange
            string taskId = "task1";
            string title = "Updated Task";
            string description = "This is an updated task";
            DateTime? dueDate = null;

            TaskItem existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Original Task",
                Description = "This is the original task",
                UserId = "USER1",
                DueDate = null,
                Status = TaskItemStatus.Pending.GetDescription()
            };

            TaskItem updatedTask = new TaskItem
            {
                Id = taskId,
                Title = title,
                Description = description,
                UserId = existingTask.UserId,
                DueDate = dueDate,
                Status = existingTask.Status
            };

            _mockTaskRepository.Setup(repo => repo.RetrieveAsync(taskId))
                .ReturnsAsync(existingTask);
            _mockTaskRepository.Setup(repo => repo.UpdateAsync(taskId, It.IsAny<TaskItem>()))
                .ReturnsAsync(updatedTask);

            // Act
            TaskItem result = await _taskService.EditTaskAsync(taskId, title, description, dueDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal(title, result.Title);
            Assert.Equal(description, result.Description);
            Assert.Equal(dueDate, result.DueDate);
        }

        [Fact]
        public async Task DeleteTaskAsync_ValidInput_ReturnsTrue()
        {
            // Arrange
            string taskId = "task1";

            _mockTaskRepository.Setup(repo => repo.DeleteAsync(taskId))
                .ReturnsAsync(true);

            // Act
            bool result = await _taskService.DeleteTaskAsync(taskId);

            // Assert
            Assert.True(result);
        }

        // Add more test methods for other scenarios and methods...

    }
}
