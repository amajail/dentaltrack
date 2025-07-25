using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Application.DTOs;

public class TreatmentDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public TreatmentType Type { get; set; }
    public string TypeDisplayName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TreatmentStatus Status { get; set; }
    public string StatusDisplayName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public PatientDto? Patient { get; set; }
    public ICollection<PhotoDto>? Photos { get; set; }
    public ICollection<AnalysisDto>? Analyses { get; set; }
    public TimeSpan? Duration { get; set; }
    public bool IsActive { get; set; }
    public bool IsCompleted { get; set; }
}

public class CreateTreatmentDto
{
    public Guid PatientId { get; set; }
    public TreatmentType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? EstimatedCost { get; set; }
    public DateTime? StartDate { get; set; }
}

public class UpdateTreatmentDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? EstimatedCost { get; set; }
}

public class CompleteTreatmentDto
{
    public decimal? ActualCost { get; set; }
    public string? Notes { get; set; }
}