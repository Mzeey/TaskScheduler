using Mzeey.DbContextLib;
using Mzeey.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzeey.SharedLib.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mzeey.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private static ConcurrentDictionary<string, TaskItem> _taskCache;
        private readonly TaskSchedulerContext _db;

        public TaskItemRepository(TaskSchedulerContext db)
        {
            _db = db;
            if(_taskCache is null)
            {
                _taskCache = new ConcurrentDictionary<string, TaskItem>(
                        _db.Tasks.ToDictionary( t => t.Id)
                    );
            }
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            task.Id = UniqueIdGenerator.GenerateUniqueId();

            await _db.Tasks.AddAsync(task);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _taskCache.AddOrUpdate(task.Id, task, updateCache) : null;
        }


        public async Task<bool> DeleteAsync(string taskId)
        {
            TaskItem taskItem = _db.Tasks.FirstOrDefault(ti => ti.Id.ToUpper() == taskId.ToUpper());
            if(taskItem is null)
            {
                return false;
            }
            _db.Tasks.Remove(taskItem);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? _taskCache.TryRemove(taskId, out taskItem) : false;
        }

        public Task<IEnumerable<TaskItem>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<TaskItem>>(() => _taskCache.Values);
        }

        public Task<TaskItem> RetrieveAsync(string taskId)
        {
            return Task.Run<TaskItem>(() =>
            {
                TaskItem taskitem;
                _taskCache.TryGetValue(taskId, out taskitem);
                return taskitem;
            });
        }

        public async Task<TaskItem> UpdateAsync(string id, TaskItem task)
        {
            if(id.ToUpper() != task.Id.ToUpper())
            {
                return null;
            }

            _db.Tasks.Update(task);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? updateCache(id, task) : null;
        }

        private TaskItem updateCache(string id, TaskItem taskItem)
        {
            return CacheUtility<string>.UpdateCache(_taskCache, id, taskItem);
        }
    }
}
