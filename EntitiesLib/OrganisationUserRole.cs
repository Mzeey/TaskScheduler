using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mzeey.Entities
{
    public class OrganisationUserRole
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string OrganisationSpaceId { get; set; }

        [ForeignKey("OrganisationSpaceId")]
        public OrganisationSpace OrganisationSpace { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
