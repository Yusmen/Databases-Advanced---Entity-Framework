

namespace P01_HospitalDatabase.Data
{

    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;
    using System;

    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfidurePatientEntity(modelBuilder);

            ConfidureVisitationEntity(modelBuilder);

            ConfidureDiagnoseEntity(modelBuilder);

            ConfidureMedicamentEntity(modelBuilder);

            ConfidurePatinetMedicamentEntity(modelBuilder);




        }

        private void ConfidurePatinetMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>()
                .HasKey(x => new { x.PatientId, x.MedicamentId });
            modelBuilder.Entity<PatientMedicament>()
                .HasOne(x => x.Patient)
                .WithMany(x => x.Prescriptions)
                .HasForeignKey(x => x.PatientId);

        modelBuilder.Entity<PatientMedicament>()
        .HasOne(x => x.Medicament)
        .WithMany(x => x.Prescriptions)
        .HasForeignKey(x => x.MedicamentId);


        }

        private void ConfidureMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>()
                .HasKey(x => x.MedicamentId);

            modelBuilder.Entity<Medicament>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();
        }

        private void ConfidureDiagnoseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnose>()
                .HasKey(x => x.DiagnoseId);

            modelBuilder.Entity<Diagnose>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder.Entity<Diagnose>()
                .Property(x => x.Comments)
                .HasMaxLength(250)
                .IsUnicode();
        }

        private void ConfidureVisitationEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visitation>()
                .HasKey(p => p.VisitationId);


            modelBuilder.Entity<Visitation>()
               .Property(x => x.Comments)
               .HasMaxLength(250)
               .IsUnicode();
            

        }

        private void ConfidurePatientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
               .Entity<Patient>()
               .Property(p => p.LastName)
               .HasMaxLength(50)
               .IsUnicode()
               .IsRequired();

            modelBuilder
               .Entity<Patient>()
               .Property(p => p.Address)
               .HasMaxLength(250)
               .IsUnicode();

            modelBuilder
               .Entity<Patient>()
               .Property(p => p.Email)
               .HasMaxLength(80);

            modelBuilder
                .Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(v => v.Patient);

            

            modelBuilder
               .Entity<Patient>()
               .HasMany(p => p.Diagnoses)
               .WithOne(pm => pm.Patient);
        }
    }
}
