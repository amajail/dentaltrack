using DentalTrack.Api.Tests.Infrastructure;
using DentalTrack.Application.DTOs;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace DentalTrack.Api.Tests.Controllers;

public class TreatmentsControllerTests : IntegrationTestBase
{
    public TreatmentsControllerTests(IntegrationTestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllTreatments_ReturnsEmptyList_WhenNoTreatmentsExist()
    {
        // Act
        var response = await Client.GetAsync("/api/treatments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var treatments = await response.Content.ReadFromJsonAsync<List<TreatmentDto>>();
        treatments.Should().NotBeNull();
        treatments.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllTreatments_ReturnsTreatments_WhenTreatmentsExist()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient();
        
        await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        });

        var treatment = TestDataFactory.CreateTestTreatment(patient.Id);
        
        await ExecuteDbContextAsync(async context =>
        {
            context.Treatments.Add(treatment);
            await context.SaveChangesAsync();
        });

        // Act
        var response = await Client.GetAsync("/api/treatments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var treatments = await response.Content.ReadFromJsonAsync<List<TreatmentDto>>();
        treatments.Should().NotBeNull();
        treatments.Should().HaveCount(1);
        treatments![0].Title.Should().Be("Teeth Whitening Treatment");
        treatments[0].Type.Should().Be(TreatmentType.Whitening);
    }

    [Fact]
    public async Task GetTreatmentById_ReturnsTreatment_WhenTreatmentExists()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient();
        
        await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        });

        var treatment = TestDataFactory.CreateTestTreatment(patient.Id);
        var treatmentId = await ExecuteDbContextAsync(async context =>
        {
            context.Treatments.Add(treatment);
            await context.SaveChangesAsync();
            return treatment.Id;
        });

        // Act
        var response = await Client.GetAsync($"/api/treatments/{treatmentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedTreatment = await response.Content.ReadFromJsonAsync<TreatmentDto>();
        returnedTreatment.Should().NotBeNull();
        returnedTreatment!.Title.Should().Be("Teeth Whitening Treatment");
        returnedTreatment.Status.Should().Be(TreatmentStatus.Planned);
    }

    [Fact]
    public async Task GetTreatmentById_ReturnsNotFound_WhenTreatmentDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/treatments/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTreatmentsByPatient_ReturnsTreatments_WhenPatientHasTreatments()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient();
        
        await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        });

        var treatment1 = TestDataFactory.CreateTestTreatment(patient.Id);
        var treatment2 = new Treatment(
            patientId: patient.Id,
            type: TreatmentType.Cleaning,
            title: "Second Treatment",
            description: "Dental cleaning",
            estimatedCost: 200m,
            startDate: DateTime.UtcNow
        );
        
        await ExecuteDbContextAsync(async context =>
        {
            context.Treatments.AddRange(treatment1, treatment2);
            await context.SaveChangesAsync();
        });

        // Act
        var response = await Client.GetAsync($"/api/treatments/patient/{patient.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var treatments = await response.Content.ReadFromJsonAsync<List<TreatmentDto>>();
        treatments.Should().NotBeNull();
        treatments.Should().HaveCount(2);
        treatments!.Should().Contain(t => t.Title == "Teeth Whitening Treatment");
        treatments.Should().Contain(t => t.Title == "Second Treatment");
    }

    [Fact]
    public async Task CreateTreatment_ReturnsCreated_WhenValidDataProvided()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient();
        await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        });

        var treatmentDto = TestDataFactory.CreateTestTreatmentDto(patient.Id);

        // Act
        var response = await Client.PostAsJsonAsync("/api/treatments", treatmentDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdTreatment = await response.Content.ReadFromJsonAsync<TreatmentDto>();
        createdTreatment.Should().NotBeNull();
        createdTreatment!.Title.Should().Be("Teeth Whitening Treatment");
        createdTreatment.PatientId.Should().Be(patient.Id);
        createdTreatment.Id.Should().NotBeEmpty();

        // Verify in database
        var dbTreatment = await ExecuteDbContextAsync(async context =>
            await context.Treatments.FirstOrDefaultAsync(t => t.PatientId == patient.Id));
        
        dbTreatment.Should().NotBeNull();
        dbTreatment!.Title.Should().Be("Teeth Whitening Treatment");
    }

    [Fact]
    public async Task CreateTreatment_ReturnsBadRequest_WhenInvalidDataProvided()
    {
        // Arrange - Missing required fields
        var invalidTreatment = new { Title = "", PatientId = Guid.Empty };

        // Act
        var response = await Client.PostAsJsonAsync("/api/treatments", invalidTreatment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTreatment_ReturnsOk_WhenValidDataProvided()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient();
        
        await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        });

        var treatment = TestDataFactory.CreateTestTreatment(patient.Id);
        var treatmentId = await ExecuteDbContextAsync(async context =>
        {
            context.Treatments.Add(treatment);
            await context.SaveChangesAsync();
            return treatment.Id;
        });

        var updateDto = new UpdateTreatmentDto
        {
            Title = "Updated Treatment",
            Description = "Updated notes"
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/treatments/{treatmentId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedTreatment = await response.Content.ReadFromJsonAsync<TreatmentDto>();
        updatedTreatment.Should().NotBeNull();
        updatedTreatment!.Title.Should().Be("Updated Treatment");
        updatedTreatment.Description.Should().Be("Updated notes");

        // Verify in database
        var dbTreatment = await ExecuteDbContextAsync(async context =>
            await context.Treatments.FirstOrDefaultAsync(t => t.Id == treatmentId));
        
        dbTreatment.Should().NotBeNull();
        dbTreatment!.Title.Should().Be("Updated Treatment");
    }

    [Fact]
    public async Task UpdateTreatment_ReturnsNotFound_WhenTreatmentDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateDto = new UpdateTreatmentDto { Title = "Test" };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/treatments/{nonExistentId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTreatment_ReturnsOk_WhenTreatmentExists()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient();
        
        await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        });

        var treatment = TestDataFactory.CreateTestTreatment(patient.Id);
        var treatmentId = await ExecuteDbContextAsync(async context =>
        {
            context.Treatments.Add(treatment);
            await context.SaveChangesAsync();
            return treatment.Id;
        });

        // Act
        var response = await Client.DeleteAsync($"/api/treatments/{treatmentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify deleted from database
        var dbTreatment = await ExecuteDbContextAsync(async context =>
            await context.Treatments.FirstOrDefaultAsync(t => t.Id == treatmentId));
        
        dbTreatment.Should().BeNull();
    }

    [Fact]
    public async Task DeleteTreatment_ReturnsNotFound_WhenTreatmentDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"/api/treatments/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}