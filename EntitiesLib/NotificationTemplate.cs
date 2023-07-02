using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    public class NotificationTemplate
    {
        [Key]
        public int Id { get; set; }
        public int NotificationTypeId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public NotificationType NotificationType { get; set; }

    }
}
