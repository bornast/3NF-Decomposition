using _3NF.Decomposition.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Persistance.Data.Configurations
{    
    public class FminMemberConfiguration : IEntityTypeConfiguration<FminMember>
    {
        public void Configure(EntityTypeBuilder<FminMember> builder)
        {
            builder.HasKey(x => new { x.RelationId, x.LeftSideMemberId, x.RightSideMemberId });

            builder.HasOne(x => x.Relation)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.RightSideMember)
                .WithMany()
                .HasForeignKey(x => x.RightSideMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.LeftSideMember)
                .WithMany()
                .HasForeignKey(x => x.LeftSideMemberId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
