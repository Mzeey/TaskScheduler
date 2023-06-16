using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mzeey.Entities
{
    public class AuthenticationToken
    {
        [Key]
        public int TokenId { get; set; }
        public string UserId { get; set; } 
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime IssuedDate { get; set; }

        public User User { get; set; }
    }
}


