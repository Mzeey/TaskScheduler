using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;

namespace Mzeey.Repositories
{
    public interface ITaskItemCommentRepository
    {
        Task<IEnumerable<TaskItemComment>> RetreiveByTaskItemIdAsync(string taskId);
        Task<TaskItemComment> RetrieveByIdAsync(string id);
        Task<TaskItemComment> CreateAsync(TaskItemComment taskItemComment);
        Task<bool> UpdateAsync(TaskItemComment taskItemComment);
        Task<bool> DeleteAsync(string id);
    }
}
