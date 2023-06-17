using System;
using System.ComponentModel.DataAnnotations;

namespace Mzeey.Entities
{
    public class TaskItemComment
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string UserId { get; set; } // The user who created the comment

        [Required]
        public string TaskItemId { get; set; } // The task associated with the comment

        public User User { get; set; } // Navigation property to the user who created the comment

        public TaskItem TaskItem { get; set; } // Navigation property to the task associated with the comment
    }
}
