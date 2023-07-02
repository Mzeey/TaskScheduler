using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mzeey.Entities
{
    public class TaskItem
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; } // Creator of the Task
        public string OrganisationSpaceId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Status { get; set; } // Possible values: "Pending", "In Progress", "Completed", "Overdue"
        public DateTime? DueDate { get; set; }
        public DateTime DateCreated { get; set; }

        public User User { get; set; }
        public OrganisationSpace OrganisationSpace { get; set; }
        public ICollection<TaskItemComment> TaskItemComments { get; set; } // Collection of comments associated with the task
        public ICollection<TaskAssignment> TaskAssignments { get; set; } // Collection of comments associated with the task
    }
}
