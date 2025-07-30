using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.ValueObjects;

public class PhotoQualityTests
{
    [Theory]
    [InlineData(PhotoQuality.Pending, "Pending Review")]
    [InlineData(PhotoQuality.Low, "Low Quality")]
    [InlineData(PhotoQuality.Medium, "Medium Quality")]
    [InlineData(PhotoQuality.High, "High Quality")]
    [InlineData(PhotoQuality.Excellent, "Excellent Quality")]
    public void GetDisplayName_ShouldReturnCorrectDisplayName(PhotoQuality quality, string expectedDisplayName)
    {
        // Act
        var displayName = quality.GetDisplayName();

        // Assert
        displayName.Should().Be(expectedDisplayName);
    }

    [Fact]
    public void GetDisplayName_WithUnknownQuality_ShouldReturnUnknown()
    {
        // Arrange
        var unknownQuality = (PhotoQuality)999;

        // Act
        var displayName = unknownQuality.GetDisplayName();

        // Assert
        displayName.Should().Be("Unknown");
    }

    [Theory]
    [InlineData(PhotoQuality.Medium, true)]
    [InlineData(PhotoQuality.High, true)]
    [InlineData(PhotoQuality.Excellent, true)]
    [InlineData(PhotoQuality.Pending, false)]
    [InlineData(PhotoQuality.Low, false)]
    public void IsAcceptable_ShouldReturnCorrectValue(PhotoQuality quality, bool expectedResult)
    {
        // Act
        var isAcceptable = quality.IsAcceptable();

        // Assert
        isAcceptable.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(PhotoQuality.Pending, true)]
    [InlineData(PhotoQuality.Low, true)]
    [InlineData(PhotoQuality.Medium, false)]
    [InlineData(PhotoQuality.High, false)]
    [InlineData(PhotoQuality.Excellent, false)]
    public void RequiresReview_ShouldReturnCorrectValue(PhotoQuality quality, bool expectedResult)
    {
        // Act
        var requiresReview = quality.RequiresReview();

        // Assert
        requiresReview.Should().Be(expectedResult);
    }

    [Fact]
    public void PhotoQuality_ShouldHaveCorrectEnumValues()
    {
        // Assert
        ((int)PhotoQuality.Pending).Should().Be(0);
        ((int)PhotoQuality.Low).Should().Be(1);
        ((int)PhotoQuality.Medium).Should().Be(2);
        ((int)PhotoQuality.High).Should().Be(3);
        ((int)PhotoQuality.Excellent).Should().Be(4);
    }

    [Fact]
    public void PhotoQuality_ShouldContainAllExpectedValues()
    {
        // Arrange
        var expectedQualities = new[]
        {
            PhotoQuality.Pending,
            PhotoQuality.Low,
            PhotoQuality.Medium,
            PhotoQuality.High,
            PhotoQuality.Excellent
        };

        // Act
        var allQualities = Enum.GetValues<PhotoQuality>();

        // Assert
        allQualities.Should().BeEquivalentTo(expectedQualities);
    }

    [Fact]
    public void ExtensionMethods_ShouldWorkWithAllEnumValues()
    {
        // Arrange
        var allQualities = Enum.GetValues<PhotoQuality>();

        // Act & Assert
        foreach (var quality in allQualities)
        {
            // These should not throw exceptions
            var displayName = quality.GetDisplayName();
            var isAcceptable = quality.IsAcceptable();
            var requiresReview = quality.RequiresReview();

            displayName.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public void BusinessLogic_ShouldBeConsistent()
    {
        // Assert business logic consistency
        // Photos that require review should not be acceptable
        PhotoQuality.Pending.RequiresReview().Should().BeTrue();
        PhotoQuality.Pending.IsAcceptable().Should().BeFalse();
        
        PhotoQuality.Low.RequiresReview().Should().BeTrue();
        PhotoQuality.Low.IsAcceptable().Should().BeFalse();
        
        // Photos that are acceptable should not require review
        PhotoQuality.Medium.IsAcceptable().Should().BeTrue();
        PhotoQuality.Medium.RequiresReview().Should().BeFalse();
        
        PhotoQuality.High.IsAcceptable().Should().BeTrue();
        PhotoQuality.High.RequiresReview().Should().BeFalse();
        
        PhotoQuality.Excellent.IsAcceptable().Should().BeTrue();
        PhotoQuality.Excellent.RequiresReview().Should().BeFalse();
    }

    [Fact]
    public void QualityLevels_ShouldBeOrderedCorrectly()
    {
        // Assert that quality levels increase in order
        ((int)PhotoQuality.Pending).Should().BeLessThan((int)PhotoQuality.Low);
        ((int)PhotoQuality.Low).Should().BeLessThan((int)PhotoQuality.Medium);
        ((int)PhotoQuality.Medium).Should().BeLessThan((int)PhotoQuality.High);
        ((int)PhotoQuality.High).Should().BeLessThan((int)PhotoQuality.Excellent);
    }

    [Fact]
    public void IsAcceptable_ShouldUseMediumAsThreshold()
    {
        // The business rule is that Medium and above are acceptable
        PhotoQuality.Medium.IsAcceptable().Should().BeTrue();
        PhotoQuality.High.IsAcceptable().Should().BeTrue();
        PhotoQuality.Excellent.IsAcceptable().Should().BeTrue();
        
        // Below Medium should not be acceptable
        PhotoQuality.Low.IsAcceptable().Should().BeFalse();
        PhotoQuality.Pending.IsAcceptable().Should().BeFalse();
    }

    [Theory]
    [InlineData(PhotoQuality.Pending)]
    [InlineData(PhotoQuality.Low)]
    public void LowQualityPhotos_ShouldRequireReview(PhotoQuality quality)
    {
        quality.RequiresReview().Should().BeTrue();
        quality.IsAcceptable().Should().BeFalse();
    }

    [Theory]
    [InlineData(PhotoQuality.Medium)]
    [InlineData(PhotoQuality.High)]
    [InlineData(PhotoQuality.Excellent)]
    public void AcceptableQualityPhotos_ShouldNotRequireReview(PhotoQuality quality)
    {
        quality.IsAcceptable().Should().BeTrue();
        quality.RequiresReview().Should().BeFalse();
    }
}