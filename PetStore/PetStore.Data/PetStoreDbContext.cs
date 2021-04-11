using Microsoft.EntityFrameworkCore;
using PetStore.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PetStore.Data
{
    public class PetStoreDbContext : DbContext
    {

        public PetStoreDbContext()
        {

        }
        public PetStoreDbContext(DbContextOptions options) 
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConfiguration.ConnectionString);

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetStoreDbContext).Assembly);
        }
    }
}
