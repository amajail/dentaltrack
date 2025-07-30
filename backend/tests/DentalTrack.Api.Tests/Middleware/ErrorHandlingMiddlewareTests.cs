using DentalTrack.Api.Tests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace DentalTrack.Api.Tests.Middleware;

public class ErrorHandlingMiddlewareTests : IntegrationTestBase
{
    public ErrorHandlingMiddlewareTests(IntegrationTestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task ErrorHandlingMiddleware_HandlesNotFoundException_Returns404()
    {
        // Act - Try to get a non-existent patient
        var response = await Client.GetAsync($"/api/patients/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ErrorHandlingMiddleware_HandlesBadRequest_Returns400()
    {
        // Act - Send invalid data to create patient
        var invalidData = new { };
        var response = await Client.PostAsJsonAsync("/api/patients", invalidData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ErrorHandlingMiddleware_HandlesInvalidGuid_Returns400()
    {
        // Act - Try to get patient with invalid GUID
        var response = await Client.GetAsync("/api/patients/not-a-guid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ErrorHandlingMiddleware_HandlesNonExistentEndpoint_Returns404()
    {
        // Act
        var response = await Client.GetAsync("/api/nonexistent-endpoint");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ErrorHandlingMiddleware_PreservesSuccessfulRequests()
    {
        // Act - Make a successful request
        var response = await Client.GetAsync("/health");

        // Assert - Should not interfere with successful requests
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Healthy");
    }

    [Fact]
    public async Task ErrorHandlingMiddleware_SetsCorrectContentType()
    {
        // Act - Trigger an error
        var response = await Client.GetAsync($"/api/patients/{Guid.NewGuid()}");

        // Assert - Should return JSON content type for API errors
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task ErrorHandlingMiddleware_HandlesEmptyPatientsList_ReturnsSuccess()
    {
        // Act - Get empty patients list (should not trigger error handling)
        var response = await Client.GetAsync("/api/patients");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("[]"); // Empty JSON array
    }

    [Fact]
    public async Task ErrorHandlingMiddleware_HandlesEmptyTreatmentsList_ReturnsSuccess()
    {
        // Act - Get empty treatments list (should not trigger error handling) 
        var response = await Client.GetAsync("/api/treatments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("[]"); // Empty JSON array
    }
}