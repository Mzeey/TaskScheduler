using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mzeey.Entities
{
    public class OrganisationUserSpace
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string OrganisationSpaceId { get; set; }
        public OrganisationSpace OrganisationSpace { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public bool IsPrivate { get; set; }
    }
}
