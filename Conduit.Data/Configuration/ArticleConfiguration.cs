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
                .HasMaxLength(10000);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.HasOne(a => a.Author)
                .WithMany(u => u.Articles)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(a => a.Tags)
                .WithMany(t => t.Articles)
                .UsingEntity<Dictionary<string, object>>(
                    x => x.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.NoAction),
                    x => x.HasOne<Article>().WithMany().HasForeignKey("ArticleId").OnDelete(DeleteBehavior.NoAction),
                    x => x.ToTable("ArticleTag"));

            builder.ToTable("Articles");
        }
    }
}