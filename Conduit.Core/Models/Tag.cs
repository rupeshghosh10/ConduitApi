using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Core.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}