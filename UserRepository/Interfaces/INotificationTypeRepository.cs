using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface INotificationTypeRepository
    {
        Task<NotificationType> CreateAsync(NotificationType notificationType);
        Task<NotificationType> UpdateAsync(int  id, NotificationType notificationType);
        Task<NotificationType> RetrieveAsync(int id);
        Task<IEnumerable<NotificationType>> RetrieveAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
