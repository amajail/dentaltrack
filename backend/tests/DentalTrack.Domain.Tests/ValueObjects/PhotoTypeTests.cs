using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.ValueObjects;

public class PhotoTypeTests
{
    [Theory]
    [InlineData(PhotoType.Intraoral, "Intraoral Photo")]
    [InlineData(PhotoType.Extraoral, "Extraoral Photo")]
    [InlineData(PhotoType.Xray, "X-Ray")]
    [InlineData(PhotoType.Panoramic, "Panoramic X-Ray")]
    [InlineData(PhotoType.Bitewing, "Bitewing X-Ray")]
    [InlineData(PhotoType.Periapical, "Periapical X-Ray")]
    [InlineData(PhotoType.Progress, "Progress Photo")]
    [InlineData(PhotoType.Before, "Before Photo")]
    [InlineData(PhotoType.After, "After Photo")]
    [InlineData(PhotoType.Clinical, "Clinical Photo")]
    [InlineData(PhotoType.Other, "Other")]
    public void GetDisplayName_ShouldReturnCorrectDisplayName(PhotoType type, string expectedDisplayName)
    {
        // Act
        var displayName = type.GetDisplayName();

        // Assert
        displayName.Should().Be(expectedDisplayName);
    }

    [Fact]
    public void GetDisplayName_WithUnknownType_ShouldReturnUnknown()
    {
        // Arrange
        var unknownType = (PhotoType)999;

        // Act
        var displayName = unknownType.GetDisplayName();

        // Assert
        displayName.Should().Be("Unknown");
    }

    [Theory]
    [InlineData(PhotoType.Xray, true)]
    [InlineData(PhotoType.Panoramic, true)]
    [InlineData(PhotoType.Bitewing, true)]
    [InlineData(PhotoType.Periapical, true)]
    [InlineData(PhotoType.Intraoral, false)]
    [InlineData(PhotoType.Extraoral, false)]
    [InlineData(PhotoType.Progress, false)]
    [InlineData(PhotoType.Before, false)]
    [InlineData(PhotoType.After, false)]
    [InlineData(PhotoType.Clinical, false)]
    [InlineData(PhotoType.Other, false)]
    public void IsXRay_ShouldReturnCorrectValue(PhotoType type, bool expectedResult)
    {
        // Act
        var isXRay = type.IsXRay();

        // Assert
        isXRay.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(PhotoType.Xray, true)]
    [InlineData(PhotoType.Panoramic, true)]
    [InlineData(PhotoType.Bitewing, true)]
    [InlineData(PhotoType.Periapical, true)]
    [InlineData(PhotoType.Intraoral, false)]
    [InlineData(PhotoType.Extraoral, false)]
    [InlineData(PhotoType.Progress, false)]
    [InlineData(PhotoType.Before, false)]
    [InlineData(PhotoType.After, false)]
    [InlineData(PhotoType.Clinical, false)]
    [InlineData(PhotoType.Other, false)]
    public void RequiresSpecialHandling_ShouldReturnCorrectValue(PhotoType type, bool expectedResult)
    {
        // Act
        var requiresSpecialHandling = type.RequiresSpecialHandling();

        // Assert
        requiresSpecialHandling.Should().Be(expectedResult);
    }

    [Fact]
    public void RequiresSpecialHandling_ShouldMatchIsXRay()
    {
        // Arrange
        var allTypes = Enum.GetValues<PhotoType>();

        // Act & Assert
        foreach (var type in allTypes)
        {
            type.RequiresSpecialHandling().Should().Be(type.IsXRay());
        }
    }

    [Fact]
    public void PhotoType_ShouldHaveCorrectEnumValues()
    {
        // Assert
        ((int)PhotoType.Intraoral).Should().Be(1);
        ((int)PhotoType.Extraoral).Should().Be(2);
        ((int)PhotoType.Xray).Should().Be(3);
        ((int)PhotoType.Panoramic).Should().Be(4);
        ((int)PhotoType.Bitewing).Should().Be(5);
        ((int)PhotoType.Periapical).Should().Be(6);
        ((int)PhotoType.Progress).Should().Be(7);
        ((int)PhotoType.Before).Should().Be(8);
        ((int)PhotoType.After).Should().Be(9);
        ((int)PhotoType.Clinical).Should().Be(10);
        ((int)PhotoType.Other).Should().Be(99);
    }

    [Fact]
    public void PhotoType_ShouldContainAllExpectedValues()
    {
        // Arrange
        var expectedTypes = new[]
        {
            PhotoType.Intraoral,
            PhotoType.Extraoral,
            PhotoType.Xray,
            PhotoType.Panoramic,
            PhotoType.Bitewing,
            PhotoType.Periapical,
            PhotoType.Progress,
            PhotoType.Before,
            PhotoType.After,
            PhotoType.Clinical,
            PhotoType.Other
        };

        // Act
        var allTypes = Enum.GetValues<PhotoType>();

        // Assert
        allTypes.Should().BeEquivalentTo(expectedTypes);
    }

    [Fact]
    public void ExtensionMethods_ShouldWorkWithAllEnumValues()
    {
        // Arrange
        var allTypes = Enum.GetValues<PhotoType>();

        // Act & Assert
        foreach (var type in allTypes)
        {
            // These should not throw exceptions
            var displayName = type.GetDisplayName();
            var isXRay = type.IsXRay();
            var requiresSpecialHandling = type.RequiresSpecialHandling();

            displayName.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public void XRayTypes_ShouldAllRequireSpecialHandling()
    {
        // Arrange
        var xrayTypes = new[]
        {
            PhotoType.Xray,
            PhotoType.Panoramic,
            PhotoType.Bitewing,
            PhotoType.Periapical
        };

        // Act & Assert
        foreach (var type in xrayTypes)
        {
            type.IsXRay().Should().BeTrue();
            type.RequiresSpecialHandling().Should().BeTrue();
        }
    }

    [Fact]
    public void NonXRayTypes_ShouldNotRequireSpecialHandling()
    {
        // Arrange
        var nonXRayTypes = new[]
        {
            PhotoType.Intraoral,
            PhotoType.Extraoral,
            PhotoType.Progress,
            PhotoType.Before,
            PhotoType.After,
            PhotoType.Clinical,
            PhotoType.Other
        };

        // Act & Assert
        foreach (var type in nonXRayTypes)
        {
            type.IsXRay().Should().BeFalse();
            type.RequiresSpecialHandling().Should().BeFalse();
        }
    }

    [Fact]
    public void XRayTypes_ShouldHaveXRayInDisplayName()
    {
        // Arrange
        var xrayTypes = new[]
        {
            PhotoType.Xray,
            PhotoType.Panoramic,
            PhotoType.Bitewing,
            PhotoType.Periapical
        };

        // Act & Assert
        foreach (var type in xrayTypes)
        {
            var displayName = type.GetDisplayName();
            displayName.Should().Contain("X-Ray", "X-Ray types should have 'X-Ray' in their display name");
        }
    }

    [Fact]
    public void BusinessLogic_ShouldBeConsistent()
    {
        // All X-Ray types should require special handling
        var allXRayTypes = Enum.GetValues<PhotoType>().Where(t => t.IsXRay());

        foreach (var type in allXRayTypes)
        {
            type.RequiresSpecialHandling().Should().BeTrue();
            type.GetDisplayName().Should().Contain("X-Ray");
        }
    }

    [Fact]
    public void OtherType_ShouldHaveSpecialValue()
    {
        // The "Other" type should have a special value (99) to distinguish it from sequential values
        ((int)PhotoType.Other).Should().Be(99);
        PhotoType.Other.IsXRay().Should().BeFalse();
        PhotoType.Other.RequiresSpecialHandling().Should().BeFalse();
        PhotoType.Other.GetDisplayName().Should().Be("Other");
    }

    [Fact]
    public void TreatmentPhotoTypes_ShouldBeIdentifiable()
    {
        // Before and After photos are common for treatment documentation
        PhotoType.Before.GetDisplayName().Should().Be("Before Photo");
        PhotoType.After.GetDisplayName().Should().Be("After Photo");
        PhotoType.Progress.GetDisplayName().Should().Be("Progress Photo");

        // These should not be X-Rays
        PhotoType.Before.IsXRay().Should().BeFalse();
        PhotoType.After.IsXRay().Should().BeFalse();
        PhotoType.Progress.IsXRay().Should().BeFalse();
    }
}