using DentalTrack.Api.Tests.Infrastructure;
using FluentAssertions;
using System.Net;

namespace DentalTrack.Api.Tests;

public class HealthControllerTests : IntegrationTestBase
{
    public HealthControllerTests(IntegrationTestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Get_Health_ReturnsOk()
    {
        // Act
        var response = await Client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Healthy");
    }

    [Fact]
    public async Task Get_Health_ReturnsHealthyString()
    {
        // Act
        var response = await Client.GetAsync("/health");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().NotBeNullOrEmpty();
        content.Should().Be("Healthy");
    }
}