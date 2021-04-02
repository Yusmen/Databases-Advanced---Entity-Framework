using Microsoft.EntityFrameworkCore;
using RealEstates.Models;

namespace RealEstates.Data
{
    public class RealEstateDbContext : DbContext
    {

        public RealEstateDbContext()
        {

        }   
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
            : base(options)
        {

        }
        public DbSet<RealEstateProperty> RealEstateProperties { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=YUSMEN-PC;Database=RealEstate;Integrated Security=true;",
                    builder => builder.EnableRetryOnFailure());
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<District>()
                .HasMany(x => x.Properties)
                .WithOne(d => d.District)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RealEstatePropertyTag>()
                .HasKey(x => new { x.PropertyId, x.TagId });


        }
    }
}
