using Mzeey.Entities;
using Mzeey.TaskSchedulerLib.Services;
using Mzeey.UserManagementLib.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    public class TaskApp : ITaskApp
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        public TaskApp(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }
        public async Task<bool> CreateTask()
        {
            bool isCreated;
            Console.WriteLine("\n--- Create Task ---");

            Console.Write("Enter task title: ");
            string title = Console.ReadLine();
            string userId = _userService.GetLoggedInUserId();

            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            Console.Write("Enter task due date (optional, format: yyyy-mm-dd): ");
            DateTime? dueDate = null;
            string dueDateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(dueDateInput))
            {
                if (DateTime.TryParse(dueDateInput, out DateTime parsedDueDate))
                {
                    dueDate = parsedDueDate;
                }
                else
                {
                    Console.WriteLine("Invalid due date format. Task will be created without a due date.");
                }
            }

            // Call the CreateTaskAsync method in the ITaskService to create the task
            TaskItem createdTask = await _taskService.CreateTaskAsync(title, description, userId, dueDate);

            if (createdTask != null)
            {
                Console.WriteLine("Task created successfully!");
                isCreated = true;
            }
            else
            {
                Console.WriteLine("Failed to create task. Please try again.");
                isCreated = false;
            }
            return isCreated;
        }

        public async Task DeleteTask()
        {
            Console.WriteLine("\n--- Delete Task ---");

            Console.Write("Enter Task ID: ");
            string taskId = Console.ReadLine().Trim();

            TaskItem task = await _taskService.GetTaskById(taskId);
            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            Console.WriteLine($"Are you sure you want to delete the task with ID {task.Id}? (Y/N)");
            string confirmation = Console.ReadLine().Trim();

            if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                bool isDeleted = await _taskService.DeleteTaskAsync(taskId);
                if (isDeleted)
                {
                    Console.WriteLine("Task deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to delete task.");
                }
            }
            else
            {
                Console.WriteLine("Task deletion canceled.");
            }
        }
        public async Task EditTask()
        {
            Console.WriteLine("\n--- Edit Task ---");

            Console.WriteLine($"Enter Task ID:");
            string taskId = Console.ReadLine().Trim();

            TaskItem task = await _taskService.GetTaskById(taskId);
            if(task == null)
            {
                Console.WriteLine("Task not found");
                return;
            }

            Console.WriteLine($"Current Title: {task.Title}");
            Console.Write("Enter new Title (leave blank to keep the current value): ");
            string title = Console.ReadLine().Trim();

            Console.WriteLine($"Current Description: {task.Description}");
            Console.Write("Enter new Description (leave blank to keep the current value): ");
            string description = Console.ReadLine().Trim();

            Console.WriteLine($"Current Due Date: {task.DueDate?.ToString("yyyy-MM-dd")}");
            Console.Write("Enter new Due Date (yyyy-MM-dd) (leave blank to keep the current value): ");
            string newDueDateStr = Console.ReadLine().Trim();

            task.Title = (!string.IsNullOrWhiteSpace(title)) ? title : task.Title;
            task.Description = (!string.IsNullOrWhiteSpace(description))? description : task.Description;

            if (!string.IsNullOrWhiteSpace(newDueDateStr))
            {
                if (DateTime.TryParseExact(newDueDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDueDate))
                {
                    task.DueDate = newDueDate;
                }
                else
                {
                    Console.WriteLine("Invalid Due Date format. Task not updated.");
                    return;
                }
            }

            TaskItem updatedTask = await _taskService.EditTaskAsync(task.Id, task.Title, task.Description, task.DueDate);
            if (updatedTask != null)
            {
                Console.WriteLine("Task updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update task.");
            }
        }

        public async Task ViewTaskDetails()
        {
            Console.WriteLine($"Enter Task ID:");
            string taskId = Console.ReadLine().Trim();

            var task = await _taskService.GetTaskById(taskId);
            if(task != null)
            {
                Console.WriteLine("\n--- My Task ---");

                Console.WriteLine($"Task ID: {task.Id}");
                Console.WriteLine($"Title: {task.Title}");
                Console.WriteLine($"Description: {task.Description}");
                Console.WriteLine($"Due Date: {task.DueDate?.ToString("yyyy-MM-dd") ?? "Not specified"}");
                Console.WriteLine($"Status: {task.Status}");
                Console.WriteLine("---------------------------------------");
            }
            else
            {
               Console.WriteLine("\nNo tasks found.");
            }
        }

        public async Task ViewMyTasks()
        {
            Console.WriteLine("\n--- My Tasks ---");
            var tasks = await _taskService.GetTasksByUserId(_userService.GetLoggedInUserId());

            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"Task ID: {task.Id}");
                    Console.WriteLine($"Title: {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Due Date: {task.DueDate?.ToString("yyyy-MM-dd") ?? "Not specified"}");
                    Console.WriteLine($"Status: {task.Status}");
                    Console.WriteLine("---------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("No tasks found.");
            }
        }

    }
}
