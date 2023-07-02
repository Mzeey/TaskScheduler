using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    public class OrganisationSpaceInvitation
    {
        [Key]
        public int Id { get; set; }
        public string OrganisationSpaceId { get; set; }
        public string InviterId { get; set; }
        public string InviteeId { get; set; }
        public int RoleId { get; set; } // Admin, Manager, User
        public string InvitationToken { get; set; }
        public string InvitationStatus { get; set; } //Accepted, Rejected
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User Inviter { get; set; }
        public User Invitee { get; set; }
        public OrganisationSpace OrganisationSpace{get;set;}
        public Role Role { get; set; }
    }
}
