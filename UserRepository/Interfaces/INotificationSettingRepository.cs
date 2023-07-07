using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface INotificationSettingRepository
    {
        Task<NotificationSetting> CreateAsync(NotificationSetting notificationSetting);
        Task<NotificationSetting> UpdateAsync(int id, NotificationSetting notificationSetting);
        Task<NotificationSetting> RetrieveAsync(int id);
        Task<IEnumerable<NotificationSetting>> RetrieveAllByUserId(string userId);
        Task<IEnumerable<NotificationSetting>> RetrieveAllAsync();
        Task<NotificationSetting> RetrieveByUserIdAndNotificationTypeId(string userId, int notificationTypeId);
        Task<bool> DeleteAsync(int id);
    }
}
