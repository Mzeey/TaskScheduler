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
    public class TaskItemRepositoryMockHelper : MockHelper<ITaskItemRepository>
    {
        public override Mock<ITaskItemRepository> ConfigureRepositoryMock()
        {
            var tasks = GenerateData<TaskItem>(10);

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TaskItem>()))
                .ReturnsAsync((TaskItem task) =>
                {
                    task.Id = Guid.NewGuid().ToString(); // Assign a new unique ID
                    tasks.Add(task);
                    return task;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<TaskItem>()))
                .ReturnsAsync((string id, TaskItem task) =>
                {
                    // Perform the update operation on the task with the provided ID
                    // and return the updated task object
                    return task;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() => tasks);

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
                .ReturnsAsync((string taskId) =>
                {
                    // Find the task with the provided ID and return it
                    // If not found, return null
                    TaskItem task = tasks.FirstOrDefault(t => t.Id == taskId);
                    return task;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync((string taskId) =>
                {
                    // Delete the task with the provided ID
                    // Return true if deletion is successful, false otherwise
                    var existingTask = tasks.FirstOrDefault(t => t.Id == taskId);
                    if (existingTask == null)
                    {
                        return false;
                    }

                    tasks.Remove(existingTask);
                    return true;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var tasks = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var taskItem = new TaskItem
                {
                    Id = GenerateUniqueId(),
                    Title = "Task " + i,
                    Description = "Description for Task-" + i,
                    Status = GetRandomTaskStatus(),
                    DateCreated = DateTime.Now
                };

                tasks.Add((T)(object) taskItem);
            }

            return tasks;
        }

        private string GetRandomTaskStatus()
        {
            var statuses = new List<string> { "Pending", "In Progress", "Completed", "Overdue" };
            var random = new Random();
            int index = random.Next(statuses.Count);
            return statuses[index];
        }
    }
}
