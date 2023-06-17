using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mzeey.Entities
{
    public class TaskItem
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public string Status { get; set; } // Possible values: "Pending", "In Progress", "Completed", "Overdue"

        public string UserId { get; set; }

        public User User { get; set; }
        
        public ICollection<TaskItemComment> Comments { get; set; } // Collection of comments associated with the task

        public string OrganizationSpaceId { get; set; }

        public OrganisationSpace OrganisationSpace { get; set; }
    }
}
