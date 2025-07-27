using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalTrack.Infrastructure.Data;

public class DataSeeder
{
    private readonly DentalTrackDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(DentalTrackDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting data seeding...");

            // Only seed if database is empty
            if (await _context.Users.AnyAsync() || await _context.Patients.AnyAsync())
            {
                _logger.LogInformation("Database already contains data. Skipping seeding.");
                return;
            }

            await SeedUsersAsync();
            await SeedPatientsAsync(); 
            await SeedTreatmentsAsync();

            await _context.SaveChangesAsync();
            _logger.LogInformation("Data seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding data");
            throw;
        }
    }

    private async Task SeedUsersAsync()
    {
        var users = new[]
        {
            new User("dr.smith@dentaltrack.com", "Dr. John", "Smith", UserRole.Doctor, "google_123456"),
            new User("dr.jones@dentaltrack.com", "Dr. Sarah", "Jones", UserRole.Doctor),
            new User("assistant@dentaltrack.com", "Jane", "Wilson", UserRole.Assistant),
            new User("admin@dentaltrack.com", "Mike", "Johnson", UserRole.Admin)
        };

        await _context.Users.AddRangeAsync(users);
        _logger.LogInformation("Seeded {Count} users", users.Length);
    }

    private async Task SeedPatientsAsync()
    {
        var patients = new[]
        {
            new Patient(
                "Alice", "Johnson", "alice.johnson@email.com", 
                new DateTime(1985, 3, 15), "555-0101", "Female",
                "123 Main St, City, State 12345",
                "Bob Johnson", "555-0102",
                "No significant medical history",
                "None known"),
            
            new Patient(
                "Bob", "Williams", "bob.williams@email.com",
                new DateTime(1978, 7, 22), "555-0201", "Male", 
                "456 Oak Ave, City, State 12345",
                "Carol Williams", "555-0202",
                "Hypertension, controlled with medication",
                "Penicillin"),

            new Patient(
                "Carol", "Brown", "carol.brown@email.com",
                new DateTime(1992, 11, 8), "555-0301", "Female",
                "789 Pine Rd, City, State 12345", 
                "David Brown", "555-0302",
                "Diabetes Type 2",
                "Latex"),

            new Patient(
                "David", "Davis", "david.davis@email.com",
                new DateTime(1965, 1, 30), "555-0401", "Male",
                "321 Elm St, City, State 12345",
                "Emma Davis", "555-0402", 
                "Heart disease, previous surgery in 2015",
                "Aspirin, Codeine"),

            new Patient(
                "Emma", "Wilson", "emma.wilson@email.com",
                new DateTime(1990, 9, 12), "555-0501", "Female",
                "654 Maple Dr, City, State 12345",
                "Frank Wilson", "555-0502",
                "No significant medical history", 
                "None known")
        };

        await _context.Patients.AddRangeAsync(patients);
        _logger.LogInformation("Seeded {Count} patients", patients.Length);
    }

    private async Task SeedTreatmentsAsync()
    {
        // First get the patients to reference them
        var patients = await _context.Patients.ToListAsync();
        if (patients.Count == 0) return;

        var treatments = new List<Treatment>();
        var random = new Random();

        // Create treatments for each patient
        foreach (var patient in patients.Take(3)) // Only first 3 patients for demo
        {
            var treatmentTypes = new[] { TreatmentType.Cleaning, TreatmentType.Filling, TreatmentType.RootCanal, TreatmentType.Crown };
            var selectedType = treatmentTypes[random.Next(treatmentTypes.Length)];
            
            var treatment = new Treatment(
                patient.Id,
                selectedType,
                GetTreatmentTitle(selectedType),
                GetTreatmentDescription(selectedType),
                GetEstimatedCost(selectedType),
                DateTime.UtcNow.AddDays(-random.Next(1, 30)));

            // Some treatments should be completed
            if (random.Next(2) == 0)
            {
                treatment.Complete(GetActualCost(selectedType), "Treatment completed successfully");
            }

            treatments.Add(treatment);
        }

        await _context.Treatments.AddRangeAsync(treatments);
        _logger.LogInformation("Seeded {Count} treatments", treatments.Count);
    }

    private static string GetTreatmentTitle(TreatmentType type) => type switch
    {
        TreatmentType.Cleaning => "Routine Dental Cleaning",
        TreatmentType.Filling => "Composite Filling",
        TreatmentType.RootCanal => "Root Canal Therapy",
        TreatmentType.Crown => "Dental Crown Placement",
        TreatmentType.Extraction => "Tooth Extraction",
        TreatmentType.Orthodontics => "Orthodontic Treatment",
        TreatmentType.Implant => "Dental Implant",
        TreatmentType.Whitening => "Teeth Whitening",
        _ => "General Treatment"
    };

    private static string GetTreatmentDescription(TreatmentType type) => type switch
    {
        TreatmentType.Cleaning => "Professional dental cleaning and examination",
        TreatmentType.Filling => "Cavity treatment with composite resin filling",
        TreatmentType.RootCanal => "Endodontic treatment to save infected tooth",
        TreatmentType.Crown => "Full coverage restoration for damaged tooth",
        TreatmentType.Extraction => "Surgical removal of problematic tooth",
        TreatmentType.Orthodontics => "Correction of teeth alignment and bite",
        TreatmentType.Implant => "Replacement of missing tooth with titanium implant",
        TreatmentType.Whitening => "Cosmetic teeth whitening procedure",
        _ => "Standard dental treatment"
    };

    private static decimal GetEstimatedCost(TreatmentType type) => type switch
    {
        TreatmentType.Cleaning => 150.00m,
        TreatmentType.Filling => 300.00m,
        TreatmentType.RootCanal => 1200.00m,
        TreatmentType.Crown => 1500.00m,
        TreatmentType.Extraction => 400.00m,
        TreatmentType.Orthodontics => 5000.00m,
        TreatmentType.Implant => 3500.00m,
        TreatmentType.Whitening => 500.00m,
        _ => 200.00m
    };

    private static decimal GetActualCost(TreatmentType type)
    {
        var estimated = GetEstimatedCost(type);
        var variance = new Random().Next(-50, 51); // -50 to +50 variance
        return Math.Max(estimated + variance, 50.00m); // Minimum $50
    }
}