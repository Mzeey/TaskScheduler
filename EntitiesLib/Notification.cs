using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string RecipientId { get; set; }
        public int NotificationTypeId { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }

        public User Recipient { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
