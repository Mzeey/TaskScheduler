using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.SharedLib.Utilities;
using NHibernate.Id;
using RepositoriesLib.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly TaskSchedulerContext _db;
        private static ConcurrentDictionary<int, TaskAssignment> _taskAssginmentCache;
        public TaskAssignmentRepository(TaskSchedulerContext db) {
            _db = db;
            if(_taskAssginmentCache == null)
            {
                _taskAssginmentCache = new ConcurrentDictionary<int, TaskAssignment>(
                     _db.TaskAssignments.ToDictionary( ta => ta.Id)
                    );
            }
        }

        public async Task<TaskAssignment> CreateAsync(TaskAssignment taskAssignment)
        {
            if(taskAssignment == null)
            {
                return null;
            }

            var addedTaskAssignment = await _db.TaskAssignments.AddAsync(taskAssignment);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _taskAssginmentCache.AddOrUpdate(taskAssignment.Id, taskAssignment, updateCache): null;
        }

        public async Task<bool> DeleteAsync(int assignmentId)
        {
            TaskAssignment existingTaskAssignment = _db.TaskAssignments.Find(assignmentId);
            if (existingTaskAssignment == null)
            {
                return false;
            }

            _db.TaskAssignments.Remove(existingTaskAssignment);
            int affected = await _db.SaveChangesAsync();
            return (affected > 0) ? _taskAssginmentCache.TryRemove(assignmentId, out existingTaskAssignment) : false;
        }

        public Task<IEnumerable<TaskAssignment>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<TaskAssignment>>(() => _taskAssginmentCache.Values);
        }

        public Task<IEnumerable<TaskAssignment>> RetrieveAllByAssigneeIdAsync(string assigneeId)
        {
            return Task.Run<IEnumerable<TaskAssignment>>(() => _taskAssginmentCache.Values.Where(ta => ta.AssigneeId.ToUpper() == assigneeId.ToUpper()));
        }

        public Task<IEnumerable<TaskAssignment>> RetrieveAllByAssignerIdAsync(string assignerId)
        {
            return Task.Run<IEnumerable<TaskAssignment>>(() => _taskAssginmentCache.Values.Where(ta => ta.AssignerId.ToUpper() == assignerId.ToUpper()));
        }

        public Task<IEnumerable<TaskAssignment>> RetrieveAllByTaskIdAsync(string taskId)
        {
            return Task.Run<IEnumerable<TaskAssignment>>(() => _taskAssginmentCache.Values.Where(ta => ta.TaskItemId.ToUpper() == taskId.ToUpper()));
        }

        public Task<TaskAssignment> RetrieveAsync(int assignmentId)
        {
            return Task.Run<TaskAssignment>(() => _taskAssginmentCache.Values.FirstOrDefault(ta => ta.Id == assignmentId));
        }


        public async Task<TaskAssignment> UpdateAsync(int assignmentId, TaskAssignment assignment)
        {
            if (assignmentId != assignment.Id)
            {
                return null;
            }

            var existingAssignment = _db.TaskAssignments.Find(assignmentId);
            if (existingAssignment is null) {
                return null;
            }

            _db.TaskAssignments.Update(assignment);

            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _taskAssginmentCache.AddOrUpdate(assignmentId, assignment, updateCache) : null;
        }

        private TaskAssignment updateCache(int id, TaskAssignment taskAssignment)
        {
            return CacheUtility<int>.UpdateCache(_taskAssginmentCache, id, taskAssignment);
        }
    }

    

        
}
