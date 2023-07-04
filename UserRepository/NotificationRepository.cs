using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.SharedLib.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly TaskSchedulerContext _db;
        private static ConcurrentDictionary<int, Notification> _notificationCache;
        public NotificationRepository(TaskSchedulerContext db)
        {
            _db = db;
            if(_notificationCache is null)
            {
                _notificationCache = new ConcurrentDictionary<int, Notification>(
                    _db.Notifications.ToDictionary(n => n.Id)
                 );
            }
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            await _db.Notifications.AddAsync(notification);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _notificationCache.AddOrUpdate(notification.Id, notification, updateCache) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Notification notification = _db.Notifications.Find(id);
            _db.Notifications.Remove(notification);
            int affected = await _db.SaveChangesAsync();
            return (affected > 0) ? _notificationCache.TryRemove(id, out notification) : false;
        }

        public Task<IEnumerable<Notification>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<Notification>>(() => _notificationCache.Values);
        }

        public Task<Notification> RetrieveAsync(int id)
        {
            return Task.Run(() =>
            {
                Notification notification;
                _notificationCache.TryGetValue(id, out notification);
                return notification;
            });
        }

        public Task<IEnumerable<Notification>> RetrieveAllByRecipientAsync(string recipientId)
        {
            return Task.Run<IEnumerable<Notification>>(() =>
            {
                IEnumerable<Notification> notifications = (IEnumerable<Notification>) _notificationCache.Where((n) => n.Value.RecipientId.ToUpper() == recipientId.ToUpper());
                return notifications;
            });
        }

        public async Task<Notification> UpdateAsync(int id, Notification notificaiton)
        {
            Notification currNotification = _db.Notifications.Find(id);
            if (currNotification == null)
            {
                return null;
            }
            _db.Notifications.Update(notificaiton);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? updateCache(id, notificaiton) : null;
        }

        private Notification updateCache(int id, Notification notification)
        {
            return CacheUtility<int>.UpdateCache(_notificationCache, id, notification);
        }
    }
}
