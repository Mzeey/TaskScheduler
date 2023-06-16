using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;
using Mzeey.TaskSchedulerLib.Repositories;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Extensions;

namespace Mzeey.TaskSchedulerLib.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskItem> CreateTaskAsync(string title, string description, string userId, DateTime? dueDate)
        {
            // Create a new task item with the provided title, description, and user ID
            TaskItem task = new TaskItem
            {
                Title = title,
                Description = description,
                UserId = userId.ToUpper(),
                DueDate = dueDate,
                DateCreated = DateTime.Now, // Set the current date and time as the creation date
                Status = TaskItemStatus.Pending.GetDescription() // Set initial status as Pending
            };

            return await _taskRepository.CreateAsync(task);
        }



        public async Task<TaskItem> EditTaskAsync(string taskId, string title, string description, DateTime? dueDate)
        {
            // Retrieve the task to be edited
            TaskItem task = await _taskRepository.RetrieveAsync(taskId);

            if (task == null)
            {
                return null; // Task not found
            }

            // Update the task properties
            task.Title = title;
            task.Description = description;
            task.DueDate = dueDate;

            return await _taskRepository.UpdateAsync(taskId, task);
        }


        public async Task<bool> DeleteTaskAsync(string taskId)
        {
            return await _taskRepository.DeleteAsync(taskId);
        }

        public async Task<bool> ChangeTaskStatusAsync(string taskId, TaskStatus newStatus)
        {
            TaskItem task = await _taskRepository.RetrieveAsync(taskId);

            if (task != null)
            {
                // Update the task status
                task.Status = newStatus.GetDescription();

                TaskItem updatedTask = await _taskRepository.UpdateAsync(taskId, task);

                return updatedTask != null;
            }

            return false; // Task not found
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            return await _taskRepository.RetrieveAllAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserId(string userId)
        {
            IEnumerable<TaskItem> tasks =  await _taskRepository.RetrieveAllAsync();
            tasks = tasks.ToList();
            tasks = tasks.Where(task => task.UserId.ToUpper() == userId.ToUpper());
            return tasks;
        }

        public async Task<TaskItem> GetTaskById(string taskId)
        {
            return await _taskRepository.RetrieveAsync(taskId);
        }
    }
}
