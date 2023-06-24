using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
        public class TaskItemCommentRepository : ITaskItemCommentRepository
        {
            private static ConcurrentDictionary<string, TaskItemComment> _commentCache;
            private readonly TaskSchedulerContext _db;

            public TaskItemCommentRepository(TaskSchedulerContext db)
            {
                _db = db;
                if (_commentCache is null)
                {
                    _commentCache = new ConcurrentDictionary<string, TaskItemComment>(
                        _db.TaskItemComments.ToDictionary(c => c.Id)
                    );
                }
            }

            public async Task<IEnumerable<TaskItemComment>> RetreiveByTaskItemIdAsync(string taskId)
            {
                return await Task.Run(() =>
                {
                    taskId = taskId.ToUpper();
                    return _commentCache.Values.Where(c => c.TaskItemId.ToUpper() == taskId);
                });
            }

            public async Task<TaskItemComment> RetrieveByIdAsync(string id)
            {
                return await Task.Run(() =>
                {
                    id = id.ToUpper();
                    return _commentCache.Values.FirstOrDefault(c => c.Id.ToUpper() == id);
                });
            }

            public async Task<TaskItemComment> CreateAsync(TaskItemComment taskItemComment)
            {
                EntityEntry<TaskItemComment> added = await _db.TaskItemComments.AddAsync(taskItemComment);
                int affected = await _db.SaveChangesAsync();

                return (affected == 1) ? _commentCache.AddOrUpdate(taskItemComment.Id, taskItemComment, updateCache) : null;
            }

            public async Task<bool> UpdateAsync(TaskItemComment taskItemComment)
            {
                _db.TaskItemComments.Update(taskItemComment);
                int affected = await _db.SaveChangesAsync();

                return affected == 1;
            }

            public async Task<bool> DeleteAsync(string id)
            {
                TaskItemComment comment = await RetrieveByIdAsync(id);
                if (comment != null)
                {
                    _db.TaskItemComments.Remove(comment);
                    int affected = await _db.SaveChangesAsync();
                    return affected == 1;
                }
                return false;
            }

            private TaskItemComment updateCache(string id, TaskItemComment taskItemComment)
            {
                return CacheUtility<string>.UpdateCache(_commentCache, id, taskItemComment);
            }
        }

}
