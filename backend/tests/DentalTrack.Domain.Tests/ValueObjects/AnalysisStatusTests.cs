using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.ValueObjects;

public class AnalysisStatusTests
{
    [Theory]
    [InlineData(AnalysisStatus.Pending, "Pending")]
    [InlineData(AnalysisStatus.Processing, "Processing")]
    [InlineData(AnalysisStatus.Completed, "Completed")]
    [InlineData(AnalysisStatus.Failed, "Failed")]
    [InlineData(AnalysisStatus.Cancelled, "Cancelled")]
    public void GetDisplayName_ShouldReturnCorrectDisplayName(AnalysisStatus status, string expectedDisplayName)
    {
        // Act
        var displayName = status.GetDisplayName();

        // Assert
        displayName.Should().Be(expectedDisplayName);
    }

    [Fact]
    public void GetDisplayName_WithUnknownStatus_ShouldReturnUnknown()
    {
        // Arrange
        var unknownStatus = (AnalysisStatus)999;

        // Act
        var displayName = unknownStatus.GetDisplayName();

        // Assert
        displayName.Should().Be("Unknown");
    }

    [Theory]
    [InlineData(AnalysisStatus.Completed, true)]
    [InlineData(AnalysisStatus.Failed, true)]
    [InlineData(AnalysisStatus.Cancelled, true)]
    [InlineData(AnalysisStatus.Pending, false)]
    [InlineData(AnalysisStatus.Processing, false)]
    public void IsFinished_ShouldReturnCorrectValue(AnalysisStatus status, bool expectedResult)
    {
        // Act
        var isFinished = status.IsFinished();

        // Assert
        isFinished.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(AnalysisStatus.Failed, true)]
    [InlineData(AnalysisStatus.Pending, false)]
    [InlineData(AnalysisStatus.Processing, false)]
    [InlineData(AnalysisStatus.Completed, false)]
    [InlineData(AnalysisStatus.Cancelled, false)]
    public void CanBeRetried_ShouldReturnCorrectValue(AnalysisStatus status, bool expectedResult)
    {
        // Act
        var canBeRetried = status.CanBeRetried();

        // Assert
        canBeRetried.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(AnalysisStatus.Processing, true)]
    [InlineData(AnalysisStatus.Pending, false)]
    [InlineData(AnalysisStatus.Completed, false)]
    [InlineData(AnalysisStatus.Failed, false)]
    [InlineData(AnalysisStatus.Cancelled, false)]
    public void IsActive_ShouldReturnCorrectValue(AnalysisStatus status, bool expectedResult)
    {
        // Act
        var isActive = status.IsActive();

        // Assert
        isActive.Should().Be(expectedResult);
    }

    [Fact]
    public void AnalysisStatus_ShouldHaveCorrectEnumValues()
    {
        // Assert
        ((int)AnalysisStatus.Pending).Should().Be(1);
        ((int)AnalysisStatus.Processing).Should().Be(2);
        ((int)AnalysisStatus.Completed).Should().Be(3);
        ((int)AnalysisStatus.Failed).Should().Be(4);
        ((int)AnalysisStatus.Cancelled).Should().Be(5);
    }

    [Fact]
    public void AnalysisStatus_ShouldContainAllExpectedValues()
    {
        // Arrange
        var expectedStatuses = new[]
        {
            AnalysisStatus.Pending,
            AnalysisStatus.Processing,
            AnalysisStatus.Completed,
            AnalysisStatus.Failed,
            AnalysisStatus.Cancelled
        };

        // Act
        var allStatuses = Enum.GetValues<AnalysisStatus>();

        // Assert
        allStatuses.Should().BeEquivalentTo(expectedStatuses);
    }

    [Fact]
    public void ExtensionMethods_ShouldWorkWithAllEnumValues()
    {
        // Arrange
        var allStatuses = Enum.GetValues<AnalysisStatus>();

        // Act & Assert
        foreach (var status in allStatuses)
        {
            // These should not throw exceptions
            var displayName = status.GetDisplayName();
            var isFinished = status.IsFinished();
            var canBeRetried = status.CanBeRetried();
            var isActive = status.IsActive();

            displayName.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public void BusinessLogic_ShouldBeConsistent()
    {
        // Assert business logic consistency
        // Only failed analyses can be retried
        AnalysisStatus.Failed.CanBeRetried().Should().BeTrue();
        AnalysisStatus.Completed.CanBeRetried().Should().BeFalse();

        // Only processing analyses are active
        AnalysisStatus.Processing.IsActive().Should().BeTrue();
        AnalysisStatus.Pending.IsActive().Should().BeFalse();

        // Finished analyses cannot be active
        AnalysisStatus.Completed.IsFinished().Should().BeTrue();
        AnalysisStatus.Completed.IsActive().Should().BeFalse();

        // Active analyses are not finished
        AnalysisStatus.Processing.IsActive().Should().BeTrue();
        AnalysisStatus.Processing.IsFinished().Should().BeFalse();
    }
}