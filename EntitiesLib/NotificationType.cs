using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    //Notification Types include {System Notificatiion, Task Notifications, Task Assignment Notifications, Organisation Space Notificaitons}
    public class NotificationType
    {
        [Key]
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<NotificationTemplate> NotificationTemplates { get; set; }
        public ICollection<NotificationSetting> NotificationSettings { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
