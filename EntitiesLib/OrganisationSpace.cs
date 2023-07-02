
using System.ComponentModel.DataAnnotations;

namespace Mzeey.Entities
{
    public class OrganisationSpace
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public ICollection<User> Users { get; set; } 

        public ICollection<TaskItem> TaskItems { get; set; }
        public ICollection<OrganisationUserSpace> OrganisationUserSpaces { get; set; } // Collection of user roles assigned to the organization space
        public ICollection<OrganisationSpaceInvitation> OrganisationSpaceInvitations { get; set; } // Collection of user roles assigned to the organization space
    }
}
