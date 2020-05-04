using _3NF.Decomposition.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Persistance.Data.Configurations
{    
    public class KeyAttributeConfiguration : IEntityTypeConfiguration<KeyAttribute>
    {
        public void Configure(EntityTypeBuilder<KeyAttribute> builder)
        {
            builder.HasKey(x => new { x.KeyId, x.AttributeId });

            builder
                .HasOne(x => x.Attribute)
                .WithMany()
                .HasForeignKey(x => x.AttributeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
