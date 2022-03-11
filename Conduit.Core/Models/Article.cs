using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Core.Models
{
    public class Article
    {
        public Article()
        {
            Comments = new List<Comment>();
            Tags = new List<Tag>();
            FavoritedUsers = new List<User>();
        }

        public int ArticleId { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<User> FavoritedUsers { get; set; }
    }
}