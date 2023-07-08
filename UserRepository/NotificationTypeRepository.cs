using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Utilities;
using NHibernate.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        private readonly TaskSchedulerContext _db;
        private static ConcurrentDictionary<int, NotificationType> _notificationTypeCache;

        public NotificationTypeRepository(TaskSchedulerContext db)
        {
            _db = db;
            if(_notificationTypeCache is null)
                _notificationTypeCache = new ConcurrentDictionary<int, NotificationType>(
                     _db.NotificationTypes.ToDictionary(nt => nt.Id)
                );
        }
        public async Task<NotificationType> CreateAsync(NotificationType notificationType)
        {
            await _db.NotificationTypes.AddAsync(notificationType);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _notificationTypeCache.AddOrUpdate(notificationType.Id, notificationType, updateCache) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            NotificationType notificationType = _db.NotificationTypes.Find(id);
            if(notificationType is null)
            {
                return false;
            }

            _db.NotificationTypes.Remove(notificationType);
            int affected = await _db.SaveChangesAsync();
            return (affected > 0) ? _notificationTypeCache.TryRemove(id, out notificationType) : false;
        }

        public Task<IEnumerable<NotificationType>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<NotificationType>>(() => _notificationTypeCache.Values);
        }

        public Task<NotificationType> RetrieveAsync(int id)
        {
            return Task.Run<NotificationType>( () =>
            {
                NotificationType notificationType;
                _notificationTypeCache.TryGetValue(id, out notificationType);
                return notificationType;
            });
        }

        public async Task<NotificationType> UpdateAsync(int id, NotificationType notificationType)
        {
            NotificationType currNotificationType = _db.NotificationTypes.Find(id);
            if (currNotificationType == null)
            {
                return null;
            }
            _db.NotificationTypes.Update(currNotificationType);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? updateCache(id, currNotificationType) : null;
        }

        private NotificationType updateCache(int id, NotificationType notificationType)
        {
            return CacheUtility<int>.UpdateCache(_notificationTypeCache, id, notificationType);
        }
    }
}
