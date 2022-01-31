using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Data.Configuration
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .UseIdentityColumn();

            builder.Property(a => a.Slug)
                .HasMaxLength(100);

            builder.HasIndex(a => a.Slug)
                .IsUnique();

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(250);

            builder.Property(a => a.Body)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.HasOne(a => a.Author)
                .WithMany(u => u.Articles)
                .HasForeignKey(u => u.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Articles");
        }
    }
}