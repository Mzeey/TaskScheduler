using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public class TaskItemCommentRepositoryMockHelper : MockHelper<ITaskItemCommentRepository>
    {
        public override Mock<ITaskItemCommentRepository> ConfigureRepositoryMock()
        {
            var comments = GenerateData<TaskItemComment>(10);

            _repositoryMock.Setup(repo => repo.RetreiveByTaskItemIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string taskId) => comments.Where(c => c.TaskItemId == taskId));

            _repositoryMock.Setup(repo => repo.RetrieveByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => comments.FirstOrDefault(c => c.Id == id));

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TaskItemComment>()))
                .ReturnsAsync((TaskItemComment comment) =>
                {
                    comments.Add(comment);
                    return comment;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<TaskItemComment>()))
                .ReturnsAsync((TaskItemComment comment) =>
                {
                    var existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
                    if (existingComment == null)
                    {
                        return false;
                    }

                    existingComment.Content = comment.Content;
                    // Update other properties as needed

                    return true;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    var existingComment = comments.FirstOrDefault(c => c.Id == id);
                    if (existingComment == null)
                    {
                        return false;
                    }

                    comments.Remove(existingComment);
                    return true;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var comments = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var comment = new TaskItemComment
                {
                    Id = Guid.NewGuid().ToString(),
                    TaskItemId = GenerateUniqueId(),
                    Content = "Comment " + i,
                    CreatedAt = DateTime.Now,
                    UserId = GenerateUniqueId()
                };

                comments.Add((T)(object) comment);
            }

            return comments;
        }
    }
}
