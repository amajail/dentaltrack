using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Entities;

public class Analysis : BaseEntity
{
    public Guid PhotoId { get; private set; }
    public Photo Photo { get; private set; }
    public AnalysisType Type { get; private set; }
    public AnalysisStatus Status { get; private set; }
    public string? Results { get; private set; }
    public decimal? ConfidenceScore { get; private set; }
    public string? Findings { get; private set; }
    public string? Recommendations { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string? ErrorMessage { get; private set; }
    public int ProcessingTimeMs { get; private set; }

    private Analysis() { }

    public Analysis(Guid photoId, AnalysisType type)
    {
        if (photoId == Guid.Empty)
            throw new ArgumentException("Photo ID cannot be empty", nameof(photoId));

        PhotoId = photoId;
        Type = type;
        Status = AnalysisStatus.Pending;
    }

    public void StartProcessing()
    {
        if (Status != AnalysisStatus.Pending)
            throw new InvalidOperationException("Analysis can only be started from Pending status");

        Status = AnalysisStatus.Processing;
        SetUpdatedAt();
    }

    public void Complete(
        string results,
        decimal? confidenceScore = null,
        string? findings = null,
        string? recommendations = null,
        int processingTimeMs = 0)
    {
        if (Status != AnalysisStatus.Processing)
            throw new InvalidOperationException("Analysis can only be completed from Processing status");
        if (string.IsNullOrWhiteSpace(results))
            throw new ArgumentException("Results cannot be empty", nameof(results));
        if (confidenceScore.HasValue && (confidenceScore < 0 || confidenceScore > 1))
            throw new ArgumentException("Confidence score must be between 0 and 1", nameof(confidenceScore));

        Status = AnalysisStatus.Completed;
        Results = results;
        ConfidenceScore = confidenceScore;
        Findings = findings;
        Recommendations = recommendations;
        CompletedAt = DateTime.UtcNow;
        ProcessingTimeMs = processingTimeMs;
        ErrorMessage = null;
        SetUpdatedAt();
    }

    public void Fail(string errorMessage)
    {
        if (Status != AnalysisStatus.Processing)
            throw new InvalidOperationException("Analysis can only be failed from Processing status");
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Error message cannot be empty", nameof(errorMessage));

        Status = AnalysisStatus.Failed;
        ErrorMessage = errorMessage;
        CompletedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Retry()
    {
        if (Status != AnalysisStatus.Failed)
            throw new InvalidOperationException("Only failed analyses can be retried");

        Status = AnalysisStatus.Pending;
        ErrorMessage = null;
        CompletedAt = null;
        Results = null;
        ConfidenceScore = null;
        Findings = null;
        Recommendations = null;
        ProcessingTimeMs = 0;
        SetUpdatedAt();
    }

    public bool IsCompleted() => Status == AnalysisStatus.Completed;
    public bool HasFailed() => Status == AnalysisStatus.Failed;
    public bool IsProcessing() => Status == AnalysisStatus.Processing;
    public bool HasHighConfidence() => ConfidenceScore.HasValue && ConfidenceScore >= 0.8m;
}