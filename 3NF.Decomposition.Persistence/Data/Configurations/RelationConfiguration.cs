using _3NF.Decomposition.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Persistance.Data.Configurations
{    
    public class RelationConfiguration : IEntityTypeConfiguration<Relation>
    {
        public void Configure(EntityTypeBuilder<Relation> builder)
        {
            builder.HasMany(x => x.FminMembers)
                .WithOne(x => x.Relation)
                .HasForeignKey(x => x.RelationId)
                .IsRequired();
        }
    }
}
