using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    public class NotificationSetting
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int NotificationTypeId { get; set; }
        public bool IsEnabled { get; set; }

        public User User { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
