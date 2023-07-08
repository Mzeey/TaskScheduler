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

namespace RepositoriesLib
{
    public class NotificationSettingRepository : INotificationSettingRepository
    {
        private readonly TaskSchedulerContext _db;
        private static ConcurrentDictionary<int, NotificationSetting> _notificationSettingsCache;

        public NotificationSettingRepository(TaskSchedulerContext db)
        {
            _db = db;
            if(_notificationSettingsCache is null )
                _notificationSettingsCache = new ConcurrentDictionary<int, NotificationSetting>(
                        _db.NotificationSettings.ToDictionary(ns => ns.Id)
                    );
        }
        public async Task<NotificationSetting> CreateAsync(NotificationSetting notificationSetting)
        {
            await _db.NotificationSettings.AddAsync(notificationSetting);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _notificationSettingsCache.AddOrUpdate(notificationSetting.Id, notificationSetting, updateCache) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingNotificationSetting = _db.NotificationSettings.Find(id);
            if(existingNotificationSetting is null)
            {
                return false;
            }
            _db.NotificationSettings.Remove(existingNotificationSetting);
            int affected = await _db.SaveChangesAsync();
            return (affected > 0) ? _notificationSettingsCache.TryRemove(id, out existingNotificationSetting) : false;
        }

        public Task<IEnumerable<NotificationSetting>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<NotificationSetting>>(() => _notificationSettingsCache.Values);
        }

        public Task<IEnumerable<NotificationSetting>> RetrieveAllByUserId(string userId)
        {
            return Task.Run<IEnumerable<NotificationSetting>>(() =>
            {
                return _notificationSettingsCache.Values.Where(ns => ns.UserId.ToUpper() == userId.ToUpper());
            }); 
        }

        public Task<NotificationSetting> RetrieveAsync(int id)
        {
            return Task.Run<NotificationSetting>(() =>
            {
                NotificationSetting notificationSetting;
                _notificationSettingsCache.TryGetValue(id, out notificationSetting);
                return notificationSetting;
            });
        }

        public Task<NotificationSetting> RetrieveByUserIdAndNotificationTypeId(string userId, int notificationTypeId)
        {
            string normalizedUserId = userId.ToUpper();
            return Task.Run<NotificationSetting>(() =>
            {
                NotificationSetting notificationSetting = _notificationSettingsCache.Values.FirstOrDefault(ns => ns.UserId.ToUpper() == normalizedUserId && ns.NotificationTypeId == notificationTypeId);
                return notificationSetting;
            });
        }

        public async Task<NotificationSetting> UpdateAsync(int id, NotificationSetting notificationSetting)
        {
            if (id != notificationSetting.Id)
                return null;
            NotificationSetting exisitingNotificationSetting = _db.NotificationSettings.FirstOrDefault(ns => ns.Id == id);
            if(exisitingNotificationSetting is null)
            {
                return null;
            }
            _db.NotificationSettings.Update(notificationSetting);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? _notificationSettingsCache.AddOrUpdate(id, notificationSetting, updateCache): null;
        }

        private NotificationSetting updateCache(int id, NotificationSetting notificationSetting)
        {
            return CacheUtility<int>.UpdateCache(_notificationSettingsCache, id, notificationSetting);
        }
    }
}
