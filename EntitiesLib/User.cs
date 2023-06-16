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
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<TaskItem> Tasks { get; set; }
        public List<AuthenticationToken> AuthenticationTokens {get;set;}
    }

}