using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;
using Mzeey.SharedLib.Enums;

namespace Mzeey.TaskSchedulerLib.Services
{
    public interface ITaskService
    {
        Task<TaskItem> CreateTaskAsync(string title, string description, string userId, DateTime? dueDate);
        Task<TaskItem> EditTaskAsync(string taskId, string title, string description, DateTime? dueDate);
        Task<bool> DeleteTaskAsync(string taskId);
        Task<bool> ChangeTaskStatusAsync(string taskId, TaskStatus newStatus);
        Task<IEnumerable<TaskItem>> GetTasksAsync();
        Task<IEnumerable<TaskItem>> GetTasksByUserId(string userId);
        Task<TaskItem> GetTaskById(string taskId);
    }
}
