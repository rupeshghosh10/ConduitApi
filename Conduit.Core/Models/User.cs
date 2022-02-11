using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Core.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<User> Followers { get; set; }

        public virtual ICollection<User> Following { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}