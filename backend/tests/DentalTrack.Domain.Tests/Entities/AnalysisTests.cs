using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Tests.Entities;

public class AnalysisTests
{
    private readonly Guid _photoId = Guid.NewGuid();
    private readonly AnalysisType _analysisType = AnalysisType.CariesDetection;

    [Fact]
    public void Constructor_WithValidInputs_CreatesAnalysis()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        Assert.Equal(_photoId, analysis.PhotoId);
        Assert.Equal(_analysisType, analysis.Type);
        Assert.Equal(AnalysisStatus.Pending, analysis.Status);
        Assert.Null(analysis.Results);
        Assert.Null(analysis.ConfidenceScore);
        Assert.Null(analysis.Findings);
        Assert.Null(analysis.Recommendations);
        Assert.Null(analysis.CompletedAt);
        Assert.Null(analysis.ErrorMessage);
        Assert.Equal(0, analysis.ProcessingTimeMs);
    }

    [Fact]
    public void Constructor_WithEmptyPhotoId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Analysis(Guid.Empty, _analysisType));
    }

    [Fact]
    public void StartProcessing_FromPendingStatus_UpdatesStatusToProcessing()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        analysis.StartProcessing();

        Assert.Equal(AnalysisStatus.Processing, analysis.Status);
    }

    [Fact]
    public void StartProcessing_FromNonPendingStatus_ThrowsInvalidOperationException()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();

        Assert.Throws<InvalidOperationException>(() => analysis.StartProcessing());
    }

    [Fact]
    public void Complete_WithValidData_CompletesAnalysis()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();
        var results = "Test results";
        var confidenceScore = 0.85m;
        var findings = "Test findings";
        var recommendations = "Test recommendations";
        var processingTime = 1500;

        analysis.Complete(results, confidenceScore, findings, recommendations, processingTime);

        Assert.Equal(AnalysisStatus.Completed, analysis.Status);
        Assert.Equal(results, analysis.Results);
        Assert.Equal(confidenceScore, analysis.ConfidenceScore);
        Assert.Equal(findings, analysis.Findings);
        Assert.Equal(recommendations, analysis.Recommendations);
        Assert.Equal(processingTime, analysis.ProcessingTimeMs);
        Assert.NotNull(analysis.CompletedAt);
        Assert.Null(analysis.ErrorMessage);
    }

    [Fact]
    public void Complete_FromNonProcessingStatus_ThrowsInvalidOperationException()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        Assert.Throws<InvalidOperationException>(() => analysis.Complete("results"));
    }

    [Fact]
    public void Complete_WithEmptyResults_ThrowsArgumentException()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();

        Assert.Throws<ArgumentException>(() => analysis.Complete(""));
        Assert.Throws<ArgumentException>(() => analysis.Complete("   "));
        Assert.Throws<ArgumentException>(() => analysis.Complete(null!));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(2.0)]
    public void Complete_WithInvalidConfidenceScore_ThrowsArgumentException(decimal invalidScore)
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();

        Assert.Throws<ArgumentException>(() => analysis.Complete("results", invalidScore));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    public void Complete_WithValidConfidenceScore_Succeeds(decimal validScore)
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();

        analysis.Complete("results", validScore);

        Assert.Equal(validScore, analysis.ConfidenceScore);
    }

    [Fact]
    public void Fail_FromProcessingStatus_FailsAnalysis()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();
        var errorMessage = "Processing failed";

        analysis.Fail(errorMessage);

        Assert.Equal(AnalysisStatus.Failed, analysis.Status);
        Assert.Equal(errorMessage, analysis.ErrorMessage);
        Assert.NotNull(analysis.CompletedAt);
    }

    [Fact]
    public void Fail_FromNonProcessingStatus_ThrowsInvalidOperationException()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        Assert.Throws<InvalidOperationException>(() => analysis.Fail("error"));
    }

    [Fact]
    public void Fail_WithEmptyErrorMessage_ThrowsArgumentException()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();

        Assert.Throws<ArgumentException>(() => analysis.Fail(""));
        Assert.Throws<ArgumentException>(() => analysis.Fail("   "));
        Assert.Throws<ArgumentException>(() => analysis.Fail(null!));
    }

    [Fact]
    public void Retry_FromFailedStatus_ResetsAnalysis()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();
        analysis.Fail("test error");

        analysis.Retry();

        Assert.Equal(AnalysisStatus.Pending, analysis.Status);
        Assert.Null(analysis.ErrorMessage);
        Assert.Null(analysis.CompletedAt);
        Assert.Null(analysis.Results);
        Assert.Null(analysis.ConfidenceScore);
        Assert.Null(analysis.Findings);
        Assert.Null(analysis.Recommendations);
        Assert.Equal(0, analysis.ProcessingTimeMs);
    }

    [Fact]
    public void Retry_FromNonFailedStatus_ThrowsInvalidOperationException()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        Assert.Throws<InvalidOperationException>(() => analysis.Retry());
    }

    [Fact]
    public void IsCompleted_WithCompletedStatus_ReturnsTrue()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();
        analysis.Complete("results");

        Assert.True(analysis.IsCompleted());
    }

    [Fact]
    public void IsCompleted_WithNonCompletedStatus_ReturnsFalse()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        Assert.False(analysis.IsCompleted());
    }

    [Fact]
    public void HasFailed_WithFailedStatus_ReturnsTrue()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();
        analysis.Fail("error");

        Assert.True(analysis.HasFailed());
    }

    [Fact]
    public void HasFailed_WithNonFailedStatus_ReturnsFalse()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        Assert.False(analysis.HasFailed());
    }

    [Fact]
    public void IsProcessing_WithProcessingStatus_ReturnsTrue()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();

        Assert.True(analysis.IsProcessing());
    }

    [Fact]
    public void IsProcessing_WithNonProcessingStatus_ReturnsFalse()
    {
        var analysis = new Analysis(_photoId, _analysisType);

        Assert.False(analysis.IsProcessing());
    }

    [Theory]
    [InlineData(0.8, true)]
    [InlineData(0.85, true)]
    [InlineData(1.0, true)]
    [InlineData(0.79, false)]
    [InlineData(0.5, false)]
    public void HasHighConfidence_WithVariousScores_ReturnsExpectedResult(double scoreValue, bool expected)
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();
        analysis.Complete("results", (decimal)scoreValue);

        Assert.Equal(expected, analysis.HasHighConfidence());
    }

    [Fact]
    public void HasHighConfidence_WithNullScore_ReturnsFalse()
    {
        var analysis = new Analysis(_photoId, _analysisType);
        analysis.StartProcessing();
        analysis.Complete("results", null);

        Assert.False(analysis.HasHighConfidence());
    }
}