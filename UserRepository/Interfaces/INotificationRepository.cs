using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> CreateAsync(Notification notification);
        Task<Notification> UpdateAsync(int id, Notification notificaiton);
        Task<IEnumerable<Notification>> RetrieveAllAsync();
        Task<Notification> RetrieveAsync(int id);
        Task<IEnumerable<Notification>> RetrieveAllByRecipientAsync(string recipientId);
        Task<bool> DeleteAsync(int id);
    }
}
