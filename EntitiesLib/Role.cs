using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public List<User> Users { get; set; }
    }
}
