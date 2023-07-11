using Moq;
using Mzeey.Entities;
using RepositoriesLib.Interfaces;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoriesLib.Tests.Repositories
{
    public class TaskAssignmentRepositoryTests
    {
        private readonly IMockHelper<ITaskAssignmentRepository> _mockHelper;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;

        public TaskAssignmentRepositoryTests()
        {
            _mockHelper = new TaskAssignmentRepositoryMockHelper();
            _taskAssignmentRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_TaskAssignment_To_Repository()
        {
            // Arrange
            var newAssignment = new TaskAssignment
            {
                AssigneeId = "assignee-id",
                AssignerId = "assigner-id",
                TaskItemId = "task-item-id"
            };

            // Act
            var createdAssignment = await _taskAssignmentRepository.CreateAsync(newAssignment);

            // Assert
            Assert.NotNull(createdAssignment);
            Assert.NotEqual(0, createdAssignment.Id);
            Assert.Equal(newAssignment.AssigneeId, createdAssignment.AssigneeId);
            Assert.Equal(newAssignment.AssignerId, createdAssignment.AssignerId);
            Assert.Equal(newAssignment.TaskItemId, createdAssignment.TaskItemId);
        }

        [Fact]
        public async Task UpdateAsync_ExistingAssignmentId_ReturnsUpdatedAssignment()
        {
            // Arrange
            var assignmentId = 1;
            var existingAssignment = await _taskAssignmentRepository.RetrieveAsync(assignmentId);

            Assert.NotNull(existingAssignment);

            // Update the assignment
            var updatedAssignment = new TaskAssignment
            {
                Id = assignmentId,
                Status = "UpdatedStatus"
            };

            // Act
            var result = await _taskAssignmentRepository.UpdateAsync(assignmentId, updatedAssignment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedAssignment.Status, result.Status);
        }

        [Fact]
        public async Task RetrieveAsync_ExistingAssignmentId_ReturnsExistingAssignment()
        {
            // Arrange
            var assignmentId = 1;

            // Act
            var existingAssignment = await _taskAssignmentRepository.RetrieveAsync(assignmentId);

            // Assert
            Assert.NotNull(existingAssignment);
            Assert.Equal(assignmentId, existingAssignment.Id);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllTaskAssignments()
        {
            // Act
            var assignments = await _taskAssignmentRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(assignments);
            Assert.Equal(10, assignments.Count());
        }

        [Fact]
        public async Task RetrieveAllByAssigneeIdAsync_ExistingAssigneeId_ReturnsAssignmentsOfAssignee()
        {
            // Arrange
            var assigneeId = "assignee-7";

            // Act
            var assignments = await _taskAssignmentRepository.RetrieveAllByAssigneeIdAsync(assigneeId);

            // Assert
            Assert.NotNull(assignments);
            Assert.NotEmpty(assignments);
            Assert.All(assignments, assignment => Assert.Equal(assigneeId, assignment.AssigneeId));
        }

        [Fact]
        public async Task RetrieveAllByAssignerIdAsync_ExistingAssignerId_ReturnsAssignmentsOfAssigner()
        {
            // Arrange
            var assignerId = "assigner-3";

            // Act
            var assignments = await _taskAssignmentRepository.RetrieveAllByAssignerIdAsync(assignerId);

            // Assert
            Assert.NotNull(assignments);
            Assert.NotEmpty(assignments);
            Assert.All(assignments, assignment => Assert.Equal(assignerId, assignment.AssignerId));
        }

        [Fact]
        public async Task RetrieveAllByTaskIdAsync_ExistingTaskId_ReturnsAssignmentsOfTask()
        {
            // Arrange
            var taskId = "task-item-4";

            // Act
            var assignments = await _taskAssignmentRepository.RetrieveAllByTaskIdAsync(taskId);

            // Assert
            Assert.NotNull(assignments);
            Assert.NotEmpty(assignments);
            Assert.All(assignments, assignment => Assert.Equal(taskId, assignment.TaskItemId));
        }


        [Fact]
        public async Task RetrieveAllByTaskIdAsync_NonExisitingTaskId_ReturnsEmptyTaskAssignmentsArray()
        {
            // Arrange
            var taskId = "task-item-id";

            // Act
            var assignments = await _taskAssignmentRepository.RetrieveAllByTaskIdAsync(taskId);

            // Assert
            Assert.NotNull(assignments);
            Assert.Empty(assignments);
        }

        [Fact]
        public async Task DeleteAsync_ExistingAssignmentId_DeletesAssignment()
        {
            // Arrange
            var assignmentId = 1;
            var existingAssignment = await _taskAssignmentRepository.RetrieveAsync(assignmentId);

            // Act
            var isDeleted = await _taskAssignmentRepository.DeleteAsync(assignmentId);

            // Assert
            Assert.True(isDeleted);
        }
    }
}
