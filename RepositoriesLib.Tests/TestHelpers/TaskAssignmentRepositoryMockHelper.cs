using Moq;
using Mzeey.Entities;
using RepositoriesLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public class TaskAssignmentRepositoryMockHelper : MockHelper<ITaskAssignmentRepository>
    {

        public TaskAssignmentRepositoryMockHelper() { 
            _repositoryMock = new Mock<ITaskAssignmentRepository>();
        }
        public override Mock<ITaskAssignmentRepository> ConfigureRepositoryMock()
        {
            var assignments = GenerateData<TaskAssignment>(10);
            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TaskAssignment>()))
                .ReturnsAsync((TaskAssignment assignment) =>
                {
                    if (assignment == null)
                        return null;
                    assignment.Id = assignments.Count + 1;
                    assignment.Status = "Pending";
                    assignment.AssignmentDate = DateTime.Now;
                    assignments.Add(assignment);
                    return assignment;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskAssignment>()))
                .ReturnsAsync((int id, TaskAssignment assignment) =>
                {
                    var existingAssignment = assignments.FirstOrDefault(ta => ta.Id == id);
                    if (existingAssignment == null)
                        return null;

                    existingAssignment = assignment;
                    return existingAssignment;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var existingAssignment = assignments.FirstOrDefault(ta => ta.Id == id);
                    if (existingAssignment == null)
                        return false;
                    return assignments.Remove(existingAssignment);
                });

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return assignments.FirstOrDefault(ta => ta.Id == id);
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllByAssigneeIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string assigneeId) =>
                {
                    return assignments.Where(ta => ta.AssigneeId.ToUpper() == assigneeId.ToUpper());
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllByAssignerIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string assignerId) =>
                {
                    return assignments.Where(ta => ta.AssignerId.ToUpper() == assignerId.ToUpper());
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() => assignments);

            _repositoryMock.Setup(repo => repo.RetrieveAllByTaskIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string taskId) =>
                {
                    return assignments.Where(ta => ta.TaskItemId.ToUpper() == taskId.ToUpper());
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var assignments = new List<T>();
            for(int i =1; i <= count; i++)
            {
                var assignment = new TaskAssignment
                {
                    Id = i,
                    AssigneeId = $"assignee-{i}",
                    AssignerId = $"assigner-{i}",
                    AssignmentDate = DateTime.Now,
                    Status = "Pending",
                    TaskItemId = $"task-item-{i}"
                };

                assignments.Add((T)(object)assignment);
            }

            return assignments;
        }
    }
}
