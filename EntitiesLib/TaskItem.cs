using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    public class TaskItem
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; } // Possible values: "Pending", "In Progress", "Completed", "Overdue"
        public string UserId { get; set; }
        public User User { get; set; }
        
    }

}
