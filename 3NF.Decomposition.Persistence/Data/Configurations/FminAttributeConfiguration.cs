using _3NF.Decomposition.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Persistance.Data.Configurations
{    
    public class FminAttributeConfiguration : IEntityTypeConfiguration<FminAttribute>
    {
        public void Configure(EntityTypeBuilder<FminAttribute> builder)
        {
            builder.HasKey(x => new { x.Id, x.RelationId, x.LeftSideAttributeId, x.RightSideAttributeId, x.Sequence });

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Relation)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.RightSideAttribute)
                .WithMany()
                .HasForeignKey(x => x.RightSideAttributeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.LeftSideAttribute)
                .WithMany()
                .HasForeignKey(x => x.LeftSideAttributeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
