using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Data.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.TagId);

            builder.Property(t => t.TagId)
                .IsRequired()
                .UseIdentityColumn();

            builder.Property(t => t.Text)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.Text)
                .IsUnique();

            builder.ToTable("Tags");
        }
    }
}