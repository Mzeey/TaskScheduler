using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface INotificationTemplateRepository
    {
        Task<NotificationTemplate> CreateAsync(NotificationTemplate notificationTemplate);
        Task<NotificationTemplate> UpdateAsync(int id, NotificationTemplate notificationTemplate);
        Task<NotificationTemplate> RetrieveAsync(int id);
        Task<NotificationTemplate> RetrieveByIdAndNotificationTypeIdAsync(int id, int notificationTypeId);
        Task<IEnumerable<NotificationTemplate>> RetrieveAllAsync();
        Task<IEnumerable<NotificationTemplate>> RetrieveAllByNotificationTypeAsync(int notificationTypeId);
        Task<bool> DeleteAsync(int id);
    }
}
