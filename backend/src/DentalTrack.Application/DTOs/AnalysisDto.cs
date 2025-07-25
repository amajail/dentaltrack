using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Application.DTOs;

public class AnalysisDto
{
    public Guid Id { get; set; }
    public Guid PhotoId { get; set; }
    public AnalysisType Type { get; set; }
    public string TypeDisplayName { get; set; } = string.Empty;
    public string TypeDescription { get; set; } = string.Empty;
    public AnalysisStatus Status { get; set; }
    public string StatusDisplayName { get; set; } = string.Empty;
    public string? Results { get; set; }
    public decimal? ConfidenceScore { get; set; }
    public string? Findings { get; set; }
    public string? Recommendations { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public int ProcessingTimeMs { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public PhotoDto? Photo { get; set; }
    public bool IsCompleted { get; set; }
    public bool HasFailed { get; set; }
    public bool IsProcessing { get; set; }
    public bool HasHighConfidence { get; set; }
    public bool IsAIBased { get; set; }
}

public class CreateAnalysisDto
{
    public Guid PhotoId { get; set; }
    public AnalysisType Type { get; set; }
}

public class CompleteAnalysisDto
{
    public string Results { get; set; } = string.Empty;
    public decimal? ConfidenceScore { get; set; }
    public string? Findings { get; set; }
    public string? Recommendations { get; set; }
    public int ProcessingTimeMs { get; set; }
}