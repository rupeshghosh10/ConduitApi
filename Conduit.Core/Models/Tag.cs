using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Core.Models
{
    public class Tag
    {
        public Tag()
        {
            Articles = new List<Article>();
        }

        public int TagId { get; set; }

        public string Text { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}