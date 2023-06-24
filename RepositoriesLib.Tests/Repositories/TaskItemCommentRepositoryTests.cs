using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using RepositoriesLib;
using RepositoriesLib.Tests.TestHelpers;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;

namespace RepositoriesLib.Tests.Repositories
{
    public class TaskItemCommentRepositoryTests
    {
        public class TaskItemCommentRepositoryTest
        {
            private readonly MockHelper mockHelper;
            private readonly ITaskItemCommentRepository taskItemCommentRepository;

            public TaskItemCommentRepositoryTest()
            {
                mockHelper = new MockHelper();
                taskItemCommentRepository = mockHelper.ConfigureTaskItemCommentRepository().Object;
            }

            [Fact]
            public async Task CreateTaskItemCommentAsync_ValidComment_ReturnsCreatedCommentWithId()
            {
                // Arrange
                var newComment = new TaskItemComment
                {
                    TaskItemId = "taskId",
                    Content = "This is a new comment."
                };

                // Act
                var createdComment = await taskItemCommentRepository.CreateAsync(newComment);

                // Assert
                Assert.NotNull(createdComment);
                Assert.Equal(newComment.TaskItemId, createdComment.TaskItemId);
                Assert.Equal(newComment.Content, createdComment.Content);
            }
        }
    }
}
