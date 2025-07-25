using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.ValueObjects;

public class TreatmentTypeTests
{
    [Theory]
    [InlineData(TreatmentType.Consultation, "Consultation")]
    [InlineData(TreatmentType.Cleaning, "Dental Cleaning")]
    [InlineData(TreatmentType.Filling, "Filling")]
    [InlineData(TreatmentType.RootCanal, "Root Canal")]
    [InlineData(TreatmentType.Crown, "Crown")]
    [InlineData(TreatmentType.Bridge, "Bridge")]
    [InlineData(TreatmentType.Implant, "Dental Implant")]
    [InlineData(TreatmentType.Extraction, "Tooth Extraction")]
    [InlineData(TreatmentType.Orthodontics, "Orthodontic Treatment")]
    [InlineData(TreatmentType.Whitening, "Teeth Whitening")]
    [InlineData(TreatmentType.Periodontal, "Periodontal Treatment")]
    [InlineData(TreatmentType.OralSurgery, "Oral Surgery")]
    [InlineData(TreatmentType.Emergency, "Emergency Treatment")]
    [InlineData(TreatmentType.Other, "Other")]
    public void GetDisplayName_ShouldReturnCorrectDisplayName(TreatmentType type, string expectedDisplayName)
    {
        // Act
        var displayName = type.GetDisplayName();

        // Assert
        displayName.Should().Be(expectedDisplayName);
    }

    [Theory]
    [InlineData(TreatmentType.RootCanal, true)]
    [InlineData(TreatmentType.Orthodontics, true)]
    [InlineData(TreatmentType.Periodontal, true)]
    [InlineData(TreatmentType.Implant, true)]
    [InlineData(TreatmentType.Consultation, false)]
    [InlineData(TreatmentType.Cleaning, false)]
    [InlineData(TreatmentType.Filling, false)]
    [InlineData(TreatmentType.Crown, false)]
    [InlineData(TreatmentType.Bridge, false)]
    [InlineData(TreatmentType.Extraction, false)]
    [InlineData(TreatmentType.Whitening, false)]
    [InlineData(TreatmentType.OralSurgery, false)]
    [InlineData(TreatmentType.Emergency, false)]
    [InlineData(TreatmentType.Other, false)]
    public void RequiresMultipleSessions_ShouldReturnCorrectValue(TreatmentType type, bool expectedResult)
    {
        // Act
        var requiresMultipleSessions = type.RequiresMultipleSessions();

        // Assert
        requiresMultipleSessions.Should().Be(expectedResult);
    }

    [Fact]
    public void GetDisplayName_WithInvalidValue_ShouldReturnUnknown()
    {
        // Arrange
        var invalidType = (TreatmentType)999;

        // Act
        var displayName = invalidType.GetDisplayName();

        // Assert
        displayName.Should().Be("Unknown");
    }

    [Fact]
    public void RequiresMultipleSessions_WithInvalidValue_ShouldReturnFalse()
    {
        // Arrange
        var invalidType = (TreatmentType)999;

        // Act
        var requiresMultipleSessions = invalidType.RequiresMultipleSessions();

        // Assert
        requiresMultipleSessions.Should().BeFalse();
    }

    [Fact]
    public void TreatmentType_ShouldHaveCorrectValues()
    {
        // Assert
        ((int)TreatmentType.Consultation).Should().Be(1);
        ((int)TreatmentType.Cleaning).Should().Be(2);
        ((int)TreatmentType.Filling).Should().Be(3);
        ((int)TreatmentType.RootCanal).Should().Be(4);
        ((int)TreatmentType.Crown).Should().Be(5);
        ((int)TreatmentType.Bridge).Should().Be(6);
        ((int)TreatmentType.Implant).Should().Be(7);
        ((int)TreatmentType.Extraction).Should().Be(8);
        ((int)TreatmentType.Orthodontics).Should().Be(9);
        ((int)TreatmentType.Whitening).Should().Be(10);
        ((int)TreatmentType.Periodontal).Should().Be(11);
        ((int)TreatmentType.OralSurgery).Should().Be(12);
        ((int)TreatmentType.Emergency).Should().Be(13);
        ((int)TreatmentType.Other).Should().Be(99);
    }
}