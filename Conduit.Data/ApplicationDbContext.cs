using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Core.Models;
using Conduit.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ArticleConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
        }
    }
}