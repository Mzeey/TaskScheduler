using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    public class TaskAssignment
    {
        [Key]
        public int Id { get; set; }
        public string TaskItemId { get; set; }
        public string AssignerId { get; set; }
        public string AssigneeId { get; set; }
        public string Status { get; set; }
        public DateTime AssignmentDate { get; set; }


        public User Assigner { get; set; }
        public User Assignee { get; set; }
        public TaskItem TaskItem { get; set; }

    }
}
