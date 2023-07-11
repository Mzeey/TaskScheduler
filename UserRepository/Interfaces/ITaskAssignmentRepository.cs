using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Interfaces
{
    public interface ITaskAssignmentRepository
    {
        Task<TaskAssignment> CreateAsync(TaskAssignment invitation);
        Task<TaskAssignment> UpdateAsync(int assignmentId, TaskAssignment assignment);
        Task<IEnumerable<TaskAssignment>> RetrieveAllAsync();
        Task<IEnumerable<TaskAssignment>> RetrieveAllByAssigneeIdAsync(string assigneeId);
        Task<IEnumerable<TaskAssignment>> RetrieveAllByAssignerIdAsync(string assignerId);
        Task<IEnumerable<TaskAssignment>> RetrieveAllByTaskIdAsync(string taskId);
        Task<TaskAssignment> RetrieveAsync(int assignmentId);
        Task<bool> DeleteAsync(int invitationId);
    }
}
