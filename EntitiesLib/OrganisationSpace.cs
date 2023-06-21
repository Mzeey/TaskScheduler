
using System.ComponentModel.DataAnnotations;

namespace Mzeey.Entities
{
    public class OrganisationSpace
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<User> Users { get; set; } 

        public ICollection<TaskItem> TaskItems { get; set; }
        public ICollection<OrganisationUserRole> OrganisationUserRoles { get; set; } // Collection of user roles assigned to the organization space
    }
}
