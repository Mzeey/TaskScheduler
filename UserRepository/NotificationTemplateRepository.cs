using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.SharedLib.Utilities;
using NHibernate.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public class NotificationTemplateRepository : INotificationTemplateRepository
    {
        private readonly TaskSchedulerContext _db;
        private static ConcurrentDictionary<int, NotificationTemplate> _notificationTemplateCache;
        public NotificationTemplateRepository(TaskSchedulerContext db) {
            _db = db;
            if(_notificationTemplateCache is null)
                _notificationTemplateCache = new ConcurrentDictionary<int, NotificationTemplate>(
                        _db.NotificationTemplates.ToDictionary(nt => nt.Id)
                    );
        }

        public async Task<NotificationTemplate> CreateAsync(NotificationTemplate notificationTemplate)
        {
            await _db.NotificationTemplates.AddAsync(notificationTemplate);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _notificationTemplateCache.AddOrUpdate(notificationTemplate.Id, notificationTemplate, updateCache) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            NotificationTemplate notificationTemplate = _db.NotificationTemplates.Find(id);
            if (notificationTemplate == null)
            {
                return false;
            }

            _db.NotificationTemplates.Remove(notificationTemplate);
            int affected = await _db.SaveChangesAsync();
            return (affected > 0) ? _notificationTemplateCache.TryRemove(id, out notificationTemplate) : false;
        }

        public Task<IEnumerable<NotificationTemplate>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<NotificationTemplate>>(() => _notificationTemplateCache.Values);
        }

        public Task<IEnumerable<NotificationTemplate>> RetrieveAllByNotificationTypeAsync(int notificationTypeId)
        {
            return Task.Run(
                () =>
                {
                    return _notificationTemplateCache.Values.Where(nt => nt.NotificationTypeId == notificationTypeId);
                });
        }

        public Task<NotificationTemplate> RetrieveAsync(int id)
        {
            return Task.Run( ()=>
            {
                NotificationTemplate notificationTemplate;
                _notificationTemplateCache.TryGetValue(id , out notificationTemplate);
                return notificationTemplate;
            });
        }

        public Task<NotificationTemplate> RetrieveByIdAndNotificationTypeIdAsync(int id, int notificationTypeId)
        {
            return Task.Run(() =>
            {
                NotificationTemplate notificationTemplate = _notificationTemplateCache.Values.FirstOrDefault(nt => nt.Id == id & nt.NotificationTypeId == notificationTypeId);
                return notificationTemplate;
            });
        }

        public async Task<NotificationTemplate> UpdateAsync(int id, NotificationTemplate notificationTemplate)
        {
            NotificationTemplate currentNotificationTemplate = _db.NotificationTemplates.Find(id);
            if(currentNotificationTemplate == null)
            {
                return null;
            }
            _db.NotificationTemplates.Update(notificationTemplate);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? updateCache(id, notificationTemplate) : null;
        }

        private NotificationTemplate updateCache(int id, NotificationTemplate notificationTemplate)
        {
            return CacheUtility<int>.UpdateCache(_notificationTemplateCache, id, notificationTemplate);
        }
    }
}
