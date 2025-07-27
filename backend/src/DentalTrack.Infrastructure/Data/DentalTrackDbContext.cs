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
    public DbSet<User> Users => Set<User>();

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
            entity.HasIndex(p => p.IsActive).HasDatabaseName("IX_Patients_IsActive");
            entity.HasIndex(p => new { p.LastName, p.FirstName }).HasDatabaseName("IX_Patients_Name");
            entity.HasIndex(p => p.CreatedAt).HasDatabaseName("IX_Patients_CreatedAt");
            entity.Property(p => p.Phone).HasMaxLength(20);
            entity.Property(p => p.Gender).HasMaxLength(10);
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

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.GoogleId).HasMaxLength(100);
            entity.HasIndex(u => u.GoogleId).IsUnique();
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Role).IsRequired().HasConversion<int>();
            entity.Property(u => u.IsActive).IsRequired();
            entity.Property(u => u.LastLoginAt);
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.Property(u => u.UpdatedAt);
        });

        // Configure indexes for better query performance
        modelBuilder.Entity<Patient>()
            .HasIndex(p => new { p.FirstName, p.LastName });

        modelBuilder.Entity<Treatment>()
            .HasIndex(t => t.Status).HasDatabaseName("IX_Treatments_Status");

        modelBuilder.Entity<Treatment>()
            .HasIndex(t => t.Type).HasDatabaseName("IX_Treatments_Type");

        modelBuilder.Entity<Treatment>()
            .HasIndex(t => t.StartDate).HasDatabaseName("IX_Treatments_StartDate");

        modelBuilder.Entity<Treatment>()
            .HasIndex(t => new { t.PatientId, t.Status }).HasDatabaseName("IX_Treatments_PatientId_Status");

        modelBuilder.Entity<Photo>()
            .HasIndex(p => p.Type).HasDatabaseName("IX_Photos_Type");

        modelBuilder.Entity<Photo>()
            .HasIndex(p => p.Quality).HasDatabaseName("IX_Photos_Quality");

        modelBuilder.Entity<Photo>()
            .HasIndex(p => p.TreatmentId).HasDatabaseName("IX_Photos_TreatmentId");

        modelBuilder.Entity<Photo>()
            .HasIndex(p => p.IsProcessed).HasDatabaseName("IX_Photos_IsProcessed");

        modelBuilder.Entity<Analysis>()
            .HasIndex(a => a.Type).HasDatabaseName("IX_Analyses_Type");

        modelBuilder.Entity<Analysis>()
            .HasIndex(a => a.Status).HasDatabaseName("IX_Analyses_Status");

        modelBuilder.Entity<Analysis>()
            .HasIndex(a => a.PhotoId).HasDatabaseName("IX_Analyses_PhotoId");

        modelBuilder.Entity<Analysis>()
            .HasIndex(a => a.CompletedAt).HasDatabaseName("IX_Analyses_CompletedAt");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Role).HasDatabaseName("IX_Users_Role");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.IsActive).HasDatabaseName("IX_Users_IsActive");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.LastLoginAt).HasDatabaseName("IX_Users_LastLoginAt");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;
            
            if (entityEntry.State == EntityState.Added)
            {
                entity.SetCreatedAt();
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entity.SetUpdatedAt();
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}