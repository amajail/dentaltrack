using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.ValueObjects;

public class AnalysisTypeTests
{
    [Theory]
    [InlineData(AnalysisType.CariesDetection, "Caries Detection")]
    [InlineData(AnalysisType.PlaqueAnalysis, "Plaque Analysis")]
    [InlineData(AnalysisType.GumHealthAssessment, "Gum Health Assessment")]
    [InlineData(AnalysisType.ToothAlignment, "Tooth Alignment Analysis")]
    [InlineData(AnalysisType.ColorMatching, "Color Matching")]
    [InlineData(AnalysisType.QualityAssessment, "Image Quality Assessment")]
    [InlineData(AnalysisType.ProgressComparison, "Progress Comparison")]
    [InlineData(AnalysisType.AnomalyDetection, "Anomaly Detection")]
    [InlineData(AnalysisType.TreatmentPlanning, "Treatment Planning")]
    [InlineData(AnalysisType.RiskAssessment, "Risk Assessment")]
    [InlineData(AnalysisType.Other, "Other Analysis")]
    public void GetDisplayName_ShouldReturnCorrectDisplayName(AnalysisType type, string expectedDisplayName)
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
        var unknownType = (AnalysisType)999;

        // Act
        var displayName = unknownType.GetDisplayName();

        // Assert
        displayName.Should().Be("Unknown");
    }

    [Theory]
    [InlineData(AnalysisType.CariesDetection, "AI-powered detection of dental caries and cavities")]
    [InlineData(AnalysisType.PlaqueAnalysis, "Analysis of plaque buildup and distribution")]
    [InlineData(AnalysisType.GumHealthAssessment, "Evaluation of gum health and periodontal conditions")]
    [InlineData(AnalysisType.ToothAlignment, "Assessment of tooth positioning and alignment")]
    [InlineData(AnalysisType.ColorMatching, "Color analysis for restorative procedures")]
    [InlineData(AnalysisType.QualityAssessment, "Automated image quality evaluation")]
    [InlineData(AnalysisType.ProgressComparison, "Comparison of treatment progress over time")]
    [InlineData(AnalysisType.AnomalyDetection, "Detection of unusual patterns or conditions")]
    [InlineData(AnalysisType.TreatmentPlanning, "AI-assisted treatment planning recommendations")]
    [InlineData(AnalysisType.RiskAssessment, "Risk factor analysis and predictions")]
    [InlineData(AnalysisType.Other, "Custom or specialized analysis")]
    public void GetDescription_ShouldReturnCorrectDescription(AnalysisType type, string expectedDescription)
    {
        // Act
        var description = type.GetDescription();

        // Assert
        description.Should().Be(expectedDescription);
    }

    [Fact]
    public void GetDescription_WithUnknownType_ShouldReturnUnknownMessage()
    {
        // Arrange
        var unknownType = (AnalysisType)999;

        // Act
        var description = unknownType.GetDescription();

        // Assert
        description.Should().Be("Unknown analysis type");
    }

    [Theory]
    [InlineData(AnalysisType.CariesDetection, true)]
    [InlineData(AnalysisType.PlaqueAnalysis, true)]
    [InlineData(AnalysisType.GumHealthAssessment, true)]
    [InlineData(AnalysisType.AnomalyDetection, true)]
    [InlineData(AnalysisType.TreatmentPlanning, true)]
    [InlineData(AnalysisType.RiskAssessment, true)]
    [InlineData(AnalysisType.ToothAlignment, false)]
    [InlineData(AnalysisType.ColorMatching, false)]
    [InlineData(AnalysisType.QualityAssessment, false)]
    [InlineData(AnalysisType.ProgressComparison, false)]
    [InlineData(AnalysisType.Other, false)]
    public void IsAIBased_ShouldReturnCorrectValue(AnalysisType type, bool expectedResult)
    {
        // Act
        var isAIBased = type.IsAIBased();

        // Assert
        isAIBased.Should().Be(expectedResult);
    }

    [Fact]
    public void IsAIBased_WithUnknownType_ShouldReturnFalse()
    {
        // Arrange
        var unknownType = (AnalysisType)999;

        // Act
        var isAIBased = unknownType.IsAIBased();

        // Assert
        isAIBased.Should().BeFalse();
    }

    [Fact]
    public void AnalysisType_ShouldHaveCorrectEnumValues()
    {
        // Assert
        ((int)AnalysisType.CariesDetection).Should().Be(1);
        ((int)AnalysisType.PlaqueAnalysis).Should().Be(2);
        ((int)AnalysisType.GumHealthAssessment).Should().Be(3);
        ((int)AnalysisType.ToothAlignment).Should().Be(4);
        ((int)AnalysisType.ColorMatching).Should().Be(5);
        ((int)AnalysisType.QualityAssessment).Should().Be(6);
        ((int)AnalysisType.ProgressComparison).Should().Be(7);
        ((int)AnalysisType.AnomalyDetection).Should().Be(8);
        ((int)AnalysisType.TreatmentPlanning).Should().Be(9);
        ((int)AnalysisType.RiskAssessment).Should().Be(10);
        ((int)AnalysisType.Other).Should().Be(99);
    }

    [Fact]
    public void AnalysisType_ShouldContainAllExpectedValues()
    {
        // Arrange
        var expectedTypes = new[]
        {
            AnalysisType.CariesDetection,
            AnalysisType.PlaqueAnalysis,
            AnalysisType.GumHealthAssessment,
            AnalysisType.ToothAlignment,
            AnalysisType.ColorMatching,
            AnalysisType.QualityAssessment,
            AnalysisType.ProgressComparison,
            AnalysisType.AnomalyDetection,
            AnalysisType.TreatmentPlanning,
            AnalysisType.RiskAssessment,
            AnalysisType.Other
        };

        // Act
        var allTypes = Enum.GetValues<AnalysisType>();

        // Assert
        allTypes.Should().BeEquivalentTo(expectedTypes);
    }

    [Fact]
    public void ExtensionMethods_ShouldWorkWithAllEnumValues()
    {
        // Arrange
        var allTypes = Enum.GetValues<AnalysisType>();

        // Act & Assert
        foreach (var type in allTypes)
        {
            // These should not throw exceptions
            var displayName = type.GetDisplayName();
            var description = type.GetDescription();
            var isAIBased = type.IsAIBased();

            displayName.Should().NotBeNullOrEmpty();
            description.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public void AIBasedTypes_ShouldBeConsistentWithDisplayNames()
    {
        // Assert that AI-based types have appropriate display names
        AnalysisType.CariesDetection.IsAIBased().Should().BeTrue();
        AnalysisType.CariesDetection.GetDisplayName().Should().Contain("Detection");

        AnalysisType.TreatmentPlanning.IsAIBased().Should().BeTrue();
        AnalysisType.TreatmentPlanning.GetDisplayName().Should().Contain("Planning");

        AnalysisType.AnomalyDetection.IsAIBased().Should().BeTrue();
        AnalysisType.AnomalyDetection.GetDisplayName().Should().Contain("Detection");
    }

    [Fact]
    public void BusinessLogic_ShouldBeConsistent()
    {
        // AI-based types should have meaningful descriptions
        var aiBasedTypes = Enum.GetValues<AnalysisType>().Where(t => t.IsAIBased());

        foreach (var type in aiBasedTypes)
        {
            var description = type.GetDescription();
            description.Should().NotBeNullOrEmpty($"AI-based type {type} should have a description");
            description.Should().NotBe("Unknown analysis type", $"AI-based type {type} should have a proper description");
        }

        // Verify specific AI-based behaviors
        AnalysisType.CariesDetection.IsAIBased().Should().BeTrue();
        AnalysisType.PlaqueAnalysis.IsAIBased().Should().BeTrue();
        AnalysisType.GumHealthAssessment.IsAIBased().Should().BeTrue();
        AnalysisType.AnomalyDetection.IsAIBased().Should().BeTrue();
        AnalysisType.TreatmentPlanning.IsAIBased().Should().BeTrue();
        AnalysisType.RiskAssessment.IsAIBased().Should().BeTrue();

        // Non-AI types should not be AI-based
        AnalysisType.ToothAlignment.IsAIBased().Should().BeFalse();
        AnalysisType.ColorMatching.IsAIBased().Should().BeFalse();
        AnalysisType.QualityAssessment.IsAIBased().Should().BeFalse();
        AnalysisType.ProgressComparison.IsAIBased().Should().BeFalse();
        AnalysisType.Other.IsAIBased().Should().BeFalse();
    }

    [Fact]
    public void OtherType_ShouldHaveSpecialValue()
    {
        // The "Other" type should have a special value (99) to distinguish it from sequential values
        ((int)AnalysisType.Other).Should().Be(99);
        AnalysisType.Other.IsAIBased().Should().BeFalse();
        AnalysisType.Other.GetDisplayName().Should().Be("Other Analysis");
    }
}