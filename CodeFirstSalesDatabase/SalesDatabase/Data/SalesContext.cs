﻿using Microsoft.EntityFrameworkCore;
using SalesDatabase.Data.Config;
using SalesDatabase.Data.Models;

namespace SalesDatabase.Data
{
    public class SalesContext : DbContext
    {

        public SalesContext()
        {

        }

        public SalesContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Connection.ConnectionString);
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity
                .Property(p => p.Name)
                .IsUnicode(true);

                entity
                .Property(p => p.Description)
                .HasDefaultValue("No Description");

            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity
                .Property(c => c.Name)
                .IsUnicode(true);

                entity
                .Property(c => c.Email)
                .IsUnicode(false);

                entity
                .Property(c => c.CreditCardNumber)
                .IsUnicode(false);


            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity
                .Property(s => s.Name)
                .IsUnicode(true);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity
                .Property(s => s.Date)
                .HasDefaultValueSql("GETDATE()");

            });


        }


    }
}
