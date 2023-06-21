using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Mzeey.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<AuthenticationToken> AuthenticationTokens {get;set;}
        public ICollection<TaskItem> Tasks { get; set; }
        public ICollection<TaskItemComment> TaskItemComments { get; set; }
        public ICollection<OrganisationUserRole> OrganisationUserRoles { get; set; } // Collection of user roles in organization spaces
    }
}