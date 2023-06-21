using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzeey.Entities;

namespace Mzeey.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem> UpdateAsync(string id, TaskItem task); 
        Task<IEnumerable<TaskItem>> RetrieveAllAsync();
        Task<TaskItem> RetrieveAsync(string taskId);
        Task<bool> DeleteAsync(string taskId);
    }
}
