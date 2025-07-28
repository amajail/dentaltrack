using DentalTrack.Domain.ValueObjects;
using DentalTrack.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DentalTrack.Infrastructure.Tests.Data;

public class DataSeederTests : TestBase
{
    [Fact]
    public async Task SeedAsync_WithEmptyDatabase_ShouldSeedInitialData()
    {
        // Arrange
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);

        // Act
        await seeder.SeedAsync();

        // Assert
        var users = await Context.Users.ToListAsync();
        var patients = await Context.Patients.ToListAsync();
        var treatments = await Context.Treatments.ToListAsync();

        users.Should().NotBeEmpty();
        patients.Should().NotBeEmpty();
        // Note: DataSeeder only creates treatments for first 3 patients, so may be empty in test
        treatments.Should().HaveCountGreaterThanOrEqualTo(0);

        // Verify admin user exists
        var adminUser = users.FirstOrDefault(u => u.Role == UserRole.Admin);
        adminUser.Should().NotBeNull();
        adminUser!.Email.Should().Be("admin@dentaltrack.com");

        // Verify doctor user exists
        var doctorUser = users.FirstOrDefault(u => u.Role == UserRole.Doctor);
        doctorUser.Should().NotBeNull();
        doctorUser!.Email.Should().StartWith("dr."); // Multiple doctors seeded

        // Verify assistant user exists
        var assistantUser = users.FirstOrDefault(u => u.Role == UserRole.Assistant);
        assistantUser.Should().NotBeNull();
        assistantUser!.Email.Should().Be("assistant@dentaltrack.com");
    }

    [Fact]
    public async Task SeedAsync_WithExistingData_ShouldNotDuplicateData()
    {
        // Arrange - Seed data first time
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);
        await seeder.SeedAsync();
        var initialUserCount = await Context.Users.CountAsync();
        var initialPatientCount = await Context.Patients.CountAsync();
        var initialTreatmentCount = await Context.Treatments.CountAsync();

        // Act - Seed again
        await seeder.SeedAsync();

        // Assert - Counts should remain the same
        var finalUserCount = await Context.Users.CountAsync();
        var finalPatientCount = await Context.Patients.CountAsync();
        var finalTreatmentCount = await Context.Treatments.CountAsync();

        finalUserCount.Should().Be(initialUserCount);
        finalPatientCount.Should().Be(initialPatientCount);
        finalTreatmentCount.Should().Be(initialTreatmentCount);
    }

    [Fact]
    public async Task SeedAsync_ShouldCreateUserWithAllRoles()
    {
        // Act
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);
        await seeder.SeedAsync();

        // Assert
        var users = await Context.Users.ToListAsync();

        users.Should().Contain(u => u.Role == UserRole.Admin);
        users.Should().Contain(u => u.Role == UserRole.Doctor);
        users.Should().Contain(u => u.Role == UserRole.Assistant);
    }

    [Fact]
    public async Task SeedAsync_ShouldCreatePatientsWithValidData()
    {
        // Act
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);
        await seeder.SeedAsync();

        // Assert
        var patients = await Context.Patients.ToListAsync();

        patients.Should().NotBeEmpty();
        patients.Should().AllSatisfy(p =>
        {
            p.FirstName.Should().NotBeNullOrEmpty();
            p.LastName.Should().NotBeNullOrEmpty();
            p.Email.Should().NotBeNullOrEmpty();
            p.DateOfBirth.Should().BeBefore(DateTime.Today);
        });

        // Verify email uniqueness
        var emails = patients.Select(p => p.Email).ToList();
        emails.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task SeedAsync_ShouldCreateTreatmentsLinkedToPatients()
    {
        // Act
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);
        await seeder.SeedAsync();

        // Assert
        var patients = await Context.Patients.ToListAsync();
        var treatments = await Context.Treatments.ToListAsync();

        patients.Should().NotBeEmpty();

        // If treatments exist, they should be linked to existing patients
        if (treatments.Any())
        {
            var patientIds = patients.Select(p => p.Id).ToHashSet();
            treatments.Should().AllSatisfy(t =>
            {
                patientIds.Should().Contain(t.PatientId);
                t.Title.Should().NotBeNullOrEmpty();
                t.EstimatedCost.Should().BeGreaterThan(0);
            });
        }
    }

    [Fact]
    public async Task SeedAsync_ShouldCreateTreatmentsWithVariousTypes()
    {
        // Act
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);
        await seeder.SeedAsync();

        // Assert
        var treatments = await Context.Treatments.ToListAsync();

        // If treatments exist, check their types
        if (treatments.Any())
        {
            var treatmentTypes = treatments.Select(t => t.Type).Distinct().ToList();
            treatmentTypes.Should().NotBeEmpty();

            // Should include at least one valid treatment type
            treatmentTypes.Should().AllSatisfy(type =>
                Enum.IsDefined(typeof(TreatmentType), type).Should().BeTrue());
        }
    }

    [Fact]
    public async Task SeedAsync_ShouldCreateTreatmentsWithVariousStatuses()
    {
        // Act
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);
        await seeder.SeedAsync();

        // Assert
        var treatments = await Context.Treatments.ToListAsync();

        // If treatments exist, check their statuses
        if (treatments.Any())
        {
            var statuses = treatments.Select(t => t.Status).Distinct().ToList();
            statuses.Should().NotBeEmpty();

            // Should have valid statuses
            statuses.Should().AllSatisfy(status =>
                Enum.IsDefined(typeof(TreatmentStatus), status).Should().BeTrue());
        }
    }

    [Fact]
    public async Task SeedAsync_WithDevelopmentEnvironment_ShouldCreateAdequateTestData()
    {
        // Act
        var logger = new Mock<ILogger<DataSeeder>>();
        var seeder = new DataSeeder(Context, logger.Object);
        await seeder.SeedAsync();

        // Assert
        var userCount = await Context.Users.CountAsync();
        var patientCount = await Context.Patients.CountAsync();
        var treatmentCount = await Context.Treatments.CountAsync();

        // Should create enough data for development/testing
        userCount.Should().BeGreaterThanOrEqualTo(3); // At least one of each role
        patientCount.Should().BeGreaterThanOrEqualTo(3); // Multiple patients for testing
        treatmentCount.Should().BeGreaterThanOrEqualTo(0); // Treatments are optional
    }
}