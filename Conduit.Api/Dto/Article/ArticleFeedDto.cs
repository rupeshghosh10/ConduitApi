using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Api.Dto.User;

namespace Conduit.Api.Dto.Article
{
    public class ArticleFeedDto
    {
        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public UserDto Author { get; set; }

        public virtual IEnumerable<string> Tags { get; set; }
    }
}