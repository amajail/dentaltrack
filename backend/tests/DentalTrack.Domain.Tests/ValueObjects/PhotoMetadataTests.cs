using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.ValueObjects;

public class PhotoMetadataTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreatePhotoMetadata()
    {
        // Arrange
        var width = 1920;
        var height = 1080;
        var cameraModel = "Canon EOS R5";
        var cameraMake = "Canon";
        var dateTaken = new DateTime(2024, 1, 15, 10, 30, 0);
        var location = "Dental Clinic Room 1";

        // Act
        var metadata = new PhotoMetadata(
            width, 
            height, 
            cameraModel, 
            cameraMake, 
            dateTaken, 
            location,
            0.125, // exposureTime
            2.8,   // fNumber
            800,   // iso
            "Auto", // flash
            85.0   // focalLength
        );

        // Assert
        metadata.Width.Should().Be(width);
        metadata.Height.Should().Be(height);
        metadata.CameraModel.Should().Be(cameraModel);
        metadata.CameraMake.Should().Be(cameraMake);
        metadata.DateTaken.Should().Be(dateTaken);
        metadata.Location.Should().Be(location);
        metadata.ExposureTime.Should().Be(0.125);
        metadata.FNumber.Should().Be(2.8);
        metadata.Iso.Should().Be(800);
        metadata.Flash.Should().Be("Auto");
        metadata.FocalLength.Should().Be(85.0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Constructor_WithInvalidWidth_ShouldThrowException(int width)
    {
        // Act & Assert
        var action = () => new PhotoMetadata(width, 1080);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Width must be positive*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Constructor_WithInvalidHeight_ShouldThrowException(int height)
    {
        // Act & Assert
        var action = () => new PhotoMetadata(1920, height);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Height must be positive*");
    }

    [Theory]
    [InlineData(1920, 1080, 1.777777777777778)] // 16:9 ratio
    [InlineData(1200, 800, 1.5)]                // 3:2 ratio
    [InlineData(1000, 1000, 1.0)]               // 1:1 ratio
    [InlineData(800, 1200, 0.6666666666666666)] // 2:3 ratio
    public void GetAspectRatio_ShouldCalculateCorrectRatio(int width, int height, double expectedRatio)
    {
        // Arrange
        var metadata = new PhotoMetadata(width, height);

        // Act
        var aspectRatio = metadata.GetAspectRatio();

        // Assert
        aspectRatio.Should().BeApproximately(expectedRatio, 0.000001);
    }

    [Theory]
    [InlineData(1920, 1080, 2073600)]   // Full HD
    [InlineData(3840, 2160, 8294400)]   // 4K
    [InlineData(800, 600, 480000)]      // Lower resolution
    public void GetTotalPixels_ShouldCalculateCorrectTotal(int width, int height, long expectedPixels)
    {
        // Arrange
        var metadata = new PhotoMetadata(width, height);

        // Act
        var totalPixels = metadata.GetTotalPixels();

        // Assert
        totalPixels.Should().Be(expectedPixels);
    }

    [Theory]
    [InlineData(1920, 1080, true)]   // 2.07MP > 2MP
    [InlineData(3840, 2160, true)]   // 8.29MP > 2MP
    [InlineData(1000, 1000, false)] // 1MP < 2MP
    [InlineData(800, 600, false)]   // 0.48MP < 2MP
    [InlineData(1500, 1334, true)]  // Exactly 2MP
    public void IsHighResolution_ShouldReturnCorrectValue(int width, int height, bool expectedResult)
    {
        // Arrange
        var metadata = new PhotoMetadata(width, height);

        // Act
        var isHighResolution = metadata.IsHighResolution();

        // Assert
        isHighResolution.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(1920, 1080, "1920x1080 (2.1MP)")]
    [InlineData(3840, 2160, "3840x2160 (8.3MP)")]
    [InlineData(800, 600, "800x600 (0.5MP)")]
    public void GetResolutionDescription_ShouldReturnFormattedString(int width, int height, string expectedDescription)
    {
        // Arrange
        var metadata = new PhotoMetadata(width, height);

        // Act
        var description = metadata.GetResolutionDescription();

        // Assert
        description.Should().Be(expectedDescription);
    }

    [Fact]
    public void Constructor_WithNullOptionalParameters_ShouldCreateValidMetadata()
    {
        // Act
        var metadata = new PhotoMetadata(1920, 1080);

        // Assert
        metadata.Width.Should().Be(1920);
        metadata.Height.Should().Be(1080);
        metadata.CameraModel.Should().BeNull();
        metadata.CameraMake.Should().BeNull();
        metadata.DateTaken.Should().BeNull();
        metadata.Location.Should().BeNull();
        metadata.ExposureTime.Should().BeNull();
        metadata.FNumber.Should().BeNull();
        metadata.Iso.Should().BeNull();
        metadata.Flash.Should().BeNull();
        metadata.FocalLength.Should().BeNull();
    }

    [Fact]
    public void PhotoMetadata_ShouldBeRecord()
    {
        // Arrange
        var metadata1 = new PhotoMetadata(1920, 1080, "Camera1", "Make1");
        var metadata2 = new PhotoMetadata(1920, 1080, "Camera1", "Make1");
        var metadata3 = new PhotoMetadata(1920, 1080, "Camera2", "Make1");

        // Assert - Records should have value equality
        metadata1.Should().Be(metadata2);
        metadata1.Should().NotBe(metadata3);
        metadata1.GetHashCode().Should().Be(metadata2.GetHashCode());
        metadata1.GetHashCode().Should().NotBe(metadata3.GetHashCode());
    }
}