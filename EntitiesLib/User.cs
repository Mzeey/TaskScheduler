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
        public bool IsEmailVerified { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public ICollection<AuthenticationToken> AuthenticationTokens {get;set;}
        public ICollection<TaskItem> Tasks { get; set; }
        public ICollection<TaskItemComment> TaskItemComments { get; set; }
        public ICollection<OrganisationUserSpace> OrganisationUserSpaces { get; set; }
    }
}