using DentalTrack.Api.Tests.Infrastructure;
using DentalTrack.Application.DTOs;
using DentalTrack.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace DentalTrack.Api.Tests.Controllers;

public class PatientsControllerTests : IntegrationTestBase
{
    public PatientsControllerTests(IntegrationTestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllPatients_ReturnsEmptyList_WhenNoPatientsExist()
    {
        // Act
        var response = await Client.GetAsync("/api/patients");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var patients = await response.Content.ReadFromJsonAsync<List<PatientDto>>();
        patients.Should().NotBeNull();
        patients.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllPatients_ReturnsPatients_WhenPatientsExist()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient("existing@test.com");
        await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        });

        // Act
        var response = await Client.GetAsync("/api/patients");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var patients = await response.Content.ReadFromJsonAsync<List<PatientDto>>();
        patients.Should().NotBeNull();
        patients.Should().HaveCount(1);
        patients![0].Email.Should().Be("existing@test.com");
        patients[0].FirstName.Should().Be("John");
        patients[0].LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task GetPatientById_ReturnsPatient_WhenPatientExists()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient("getbyid@test.com");
        var patientId = await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            return patient.Id;
        });

        // Act
        var response = await Client.GetAsync($"/api/patients/{patientId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedPatient = await response.Content.ReadFromJsonAsync<PatientDto>();
        returnedPatient.Should().NotBeNull();
        returnedPatient!.Email.Should().Be("getbyid@test.com");
        returnedPatient.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task GetPatientById_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/patients/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreatePatient_ReturnsCreated_WhenValidDataProvided()
    {
        // Arrange
        var patientDto = TestDataFactory.CreateTestPatientDto("create@test.com");

        // Act
        var response = await Client.PostAsJsonAsync("/api/patients", patientDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdPatient = await response.Content.ReadFromJsonAsync<PatientDto>();
        createdPatient.Should().NotBeNull();
        createdPatient!.Email.Should().Be("create@test.com");
        createdPatient.Id.Should().NotBeEmpty();

        // Verify in database
        var dbPatient = await ExecuteDbContextAsync(async context =>
            await context.Patients.FirstOrDefaultAsync(p => p.Email == "create@test.com"));
        
        dbPatient.Should().NotBeNull();
        dbPatient!.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task CreatePatient_ReturnsBadRequest_WhenInvalidDataProvided()
    {
        // Arrange - Missing required fields
        var invalidPatient = new { FirstName = "", Email = "invalid-email" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/patients", invalidPatient);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdatePatient_ReturnsOk_WhenValidDataProvided()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient("update@test.com");
        var patientId = await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            return patient.Id;
        });

        var updateDto = TestDataFactory.CreateTestPatientDto("updated@test.com");
        updateDto.FirstName = "Updated";

        // Act
        var response = await Client.PutAsJsonAsync($"/api/patients/{patientId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedPatient = await response.Content.ReadFromJsonAsync<PatientDto>();
        updatedPatient.Should().NotBeNull();
        updatedPatient!.FirstName.Should().Be("Updated");
        updatedPatient.Email.Should().Be("updated@test.com");

        // Verify in database
        var dbPatient = await ExecuteDbContextAsync(async context =>
            await context.Patients.FirstOrDefaultAsync(p => p.Id == patientId));
        
        dbPatient.Should().NotBeNull();
        dbPatient!.FirstName.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdatePatient_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateDto = TestDataFactory.CreateTestPatientDto();

        // Act
        var response = await Client.PutAsJsonAsync($"/api/patients/{nonExistentId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePatient_ReturnsOk_WhenPatientExists()
    {
        // Arrange
        var patient = TestDataFactory.CreateTestPatient("delete@test.com");
        var patientId = await ExecuteDbContextAsync(async context =>
        {
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            return patient.Id;
        });

        // Act
        var response = await Client.DeleteAsync($"/api/patients/{patientId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify deleted from database
        var dbPatient = await ExecuteDbContextAsync(async context =>
            await context.Patients.FirstOrDefaultAsync(p => p.Id == patientId));
        
        dbPatient.Should().BeNull();
    }

    [Fact]
    public async Task DeletePatient_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"/api/patients/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}