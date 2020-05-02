using _3NF.Decomposition.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Persistance.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Relation> Relations { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<KeyMember> KeyMembers { get; set; }
        public DbSet<FminMember> FminMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // apply all configurations from Data/Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

    }
}
