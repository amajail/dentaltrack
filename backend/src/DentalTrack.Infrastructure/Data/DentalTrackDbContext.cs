using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DentalTrack.Infrastructure.Data;

public class DentalTrackDbContext : DbContext
{
    public DentalTrackDbContext(DbContextOptions<DentalTrackDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Treatment> Treatments => Set<Treatment>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<Analysis> Analyses => Set<Analysis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Patient entity
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(p => p.Email).IsUnique();
            entity.Property(p => p.Phone).HasMaxLength(20);
            entity.Property(p => p.Address).HasMaxLength(500);
            entity.Property(p => p.EmergencyContact).HasMaxLength(100);
            entity.Property(p => p.EmergencyPhone).HasMaxLength(20);
            entity.Property(p => p.MedicalHistory).HasMaxLength(2000);
            entity.Property(p => p.Allergies).HasMaxLength(1000);
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.Property(p => p.UpdatedAt);
            entity.Property(p => p.IsActive).IsRequired();

            // Configure relationships
            entity.HasMany(p => p.Treatments)
                  .WithOne(t => t.Patient)
                  .HasForeignKey(t => t.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Treatment entity
        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.PatientId).IsRequired();
            entity.Property(t => t.Type).IsRequired().HasConversion<int>();
            entity.Property(t => t.Status).IsRequired().HasConversion<int>();
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(1000);
            entity.Property(t => t.StartDate).IsRequired();
            entity.Property(t => t.EndDate);
            entity.Property(t => t.EstimatedCost).HasColumnType("decimal(10,2)");
            entity.Property(t => t.ActualCost).HasColumnType("decimal(10,2)");
            entity.Property(t => t.Notes).HasMaxLength(2000);
            entity.Property(t => t.CreatedAt).IsRequired();
            entity.Property(t => t.UpdatedAt);

            // Configure relationships
            entity.HasMany(t => t.Photos)
                  .WithOne(p => p.Treatment)
                  .HasForeignKey(p => p.TreatmentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(t => t.Analyses)
                  .WithOne()
                  .HasForeignKey("TreatmentId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Photo entity
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.TreatmentId).IsRequired();
            entity.Property(p => p.FileName).IsRequired().HasMaxLength(255);
            entity.Property(p => p.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(p => p.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(p => p.FileSize).IsRequired();
            entity.Property(p => p.Type).IsRequired().HasConversion<int>();
            entity.Property(p => p.Quality).IsRequired().HasConversion<int>();
            entity.Property(p => p.Description).HasMaxLength(500);
            entity.Property(p => p.ToothNumber);
            entity.Property(p => p.IsProcessed).IsRequired();
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.Property(p => p.UpdatedAt);

            // Configure owned type PhotoMetadata
            entity.OwnsOne(p => p.Metadata, metadata =>
            {
                metadata.Property(m => m.Width).IsRequired();
                metadata.Property(m => m.Height).IsRequired();
                metadata.Property(m => m.CameraModel).HasMaxLength(100);
                metadata.Property(m => m.CameraMake).HasMaxLength(100);
                metadata.Property(m => m.DateTaken);
                metadata.Property(m => m.Location).HasMaxLength(200);
                metadata.Property(m => m.ExposureTime);
                metadata.Property(m => m.FNumber);
                metadata.Property(m => m.Iso);
                metadata.Property(m => m.Flash).HasMaxLength(50);
                metadata.Property(m => m.FocalLength);
            });

            // Configure relationships
            entity.HasMany(p => p.Analyses)
                  .WithOne(a => a.Photo)
                  .HasForeignKey(a => a.PhotoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Analysis entity
        modelBuilder.Entity<Analysis>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.PhotoId).IsRequired();
            entity.Property(a => a.Type).IsRequired().HasConversion<int>();
            entity.Property(a => a.Status).IsRequired().HasConversion<int>();
            entity.Property(a => a.Results).HasMaxLength(5000);
            entity.Property(a => a.ConfidenceScore).HasColumnType("decimal(5,4)");
            entity.Property(a => a.Findings).HasMaxLength(2000);
            entity.Property(a => a.Recommendations).HasMaxLength(2000);
            entity.Property(a => a.CompletedAt);
            entity.Property(a => a.ErrorMessage).HasMaxLength(1000);
            entity.Property(a => a.ProcessingTimeMs).IsRequired();
            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.UpdatedAt);
        });

        // Configure indexes for better query performance
        modelBuilder.Entity<Patient>()
            .HasIndex(p => new { p.FirstName, p.LastName });

        modelBuilder.Entity<Treatment>()
            .HasIndex(t => t.Status);

        modelBuilder.Entity<Treatment>()
            .HasIndex(t => t.Type);

        modelBuilder.Entity<Photo>()
            .HasIndex(p => p.Type);

        modelBuilder.Entity<Photo>()
            .HasIndex(p => p.Quality);

        modelBuilder.Entity<Analysis>()
            .HasIndex(a => a.Type);

        modelBuilder.Entity<Analysis>()
            .HasIndex(a => a.Status);
    }
}