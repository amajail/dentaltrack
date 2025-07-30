using DentalTrack.Api.Tests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace DentalTrack.Api.Tests.Controllers;

public class ErrorHandlingTests : IntegrationTestBase
{
    public ErrorHandlingTests(IntegrationTestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task CreatePatient_ReturnsBadRequest_WhenMissingRequiredFields()
    {
        // Arrange - Missing FirstName and LastName
        var invalidPatient = new
        {
            Email = "test@example.com",
            Phone = "555-0123"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/patients", invalidPatient);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreatePatient_ReturnsBadRequest_WhenInvalidEmailFormat()
    {
        // Arrange
        var invalidPatient = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "not-an-email",
            Phone = "555-0123"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/patients", invalidPatient);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTreatment_ReturnsBadRequest_WhenMissingPatientId()
    {
        // Arrange
        var invalidTreatment = new
        {
            Title = "Test Treatment",
            Type = "Professional",
            ExpectedSessions = 3
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/treatments", invalidTreatment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTreatment_ReturnsBadRequest_WhenEmptyTitle()
    {
        // Arrange
        var invalidTreatment = new
        {
            Title = "",
            PatientId = Guid.NewGuid(),
            Type = "Professional",
            ExpectedSessions = 3
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/treatments", invalidTreatment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTreatment_ReturnsBadRequest_WhenInvalidExpectedSessions()
    {
        // Arrange
        var invalidTreatment = new
        {
            Title = "Test Treatment",
            PatientId = Guid.NewGuid(),
            Type = "Professional",
            ExpectedSessions = 0 // Invalid - should be positive
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/treatments", invalidTreatment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetPatient_ReturnsBadRequest_WhenInvalidGuidFormat()
    {
        // Act
        var response = await Client.GetAsync("/api/patients/not-a-guid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTreatment_ReturnsBadRequest_WhenInvalidGuidFormat()
    {
        // Act
        var response = await Client.GetAsync("/api/treatments/not-a-guid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePatient_ReturnsBadRequest_WhenFutureDateOfBirth()
    {
        // Arrange
        var invalidPatient = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Phone = "555-0123",
            DateOfBirth = DateTime.UtcNow.AddYears(1) // Future date
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/patients", invalidPatient);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePatient_ReturnsBadRequest_WhenPhoneTooShort()
    {
        // Arrange
        var invalidPatient = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Phone = "123" // Too short
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/patients", invalidPatient);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Api_ReturnsNotFound_WhenEndpointDoesNotExist()
    {
        // Act
        var response = await Client.GetAsync("/api/nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}