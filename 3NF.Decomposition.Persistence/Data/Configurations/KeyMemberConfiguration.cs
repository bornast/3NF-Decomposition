using _3NF.Decomposition.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Persistance.Data.Configurations
{    
    public class KeyMemberConfiguration : IEntityTypeConfiguration<KeyMember>
    {
        public void Configure(EntityTypeBuilder<KeyMember> builder)
        {
            builder.HasKey(x => new { x.KeyId, x.MemberId });

            builder
                .HasOne(x => x.Member)
                .WithMany()
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
