using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Entities;

public class Treatment : BaseEntity
{
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; }
    public TreatmentType Type { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public TreatmentStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public decimal? EstimatedCost { get; private set; }
    public decimal? ActualCost { get; private set; }
    public string? Notes { get; private set; }

    public ICollection<Photo> Photos { get; private set; } = new List<Photo>();
    public ICollection<Analysis> Analyses { get; private set; } = new List<Analysis>();

    private Treatment() { }

    public Treatment(
        Guid patientId,
        TreatmentType type,
        string title,
        string? description = null,
        decimal? estimatedCost = null,
        DateTime? startDate = null)
    {
        if (patientId == Guid.Empty)
            throw new ArgumentException("Patient ID cannot be empty", nameof(patientId));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        PatientId = patientId;
        Type = type;
        Title = title;
        Description = description;
        EstimatedCost = estimatedCost;
        StartDate = startDate ?? DateTime.UtcNow;
        Status = TreatmentStatus.Planned;
    }

    public void Start()
    {
        if (Status != TreatmentStatus.Planned)
            throw new InvalidOperationException("Treatment can only be started from Planned status");

        Status = TreatmentStatus.InProgress;
        StartDate = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Complete(decimal? actualCost = null, string? notes = null)
    {
        if (Status != TreatmentStatus.InProgress)
            throw new InvalidOperationException("Treatment can only be completed from InProgress status");

        Status = TreatmentStatus.Completed;
        EndDate = DateTime.UtcNow;
        ActualCost = actualCost;
        Notes = notes;
        SetUpdatedAt();
    }

    public void Cancel(string? reason = null)
    {
        if (Status == TreatmentStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed treatment");

        Status = TreatmentStatus.Cancelled;
        Notes = reason;
        SetUpdatedAt();
    }

    public void UpdateDetails(
        string title,
        string? description = null,
        decimal? estimatedCost = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title;
        Description = description;
        EstimatedCost = estimatedCost;
        SetUpdatedAt();
    }

    public TimeSpan? GetDuration()
    {
        if (EndDate.HasValue)
            return EndDate.Value - StartDate;

        if (Status == TreatmentStatus.InProgress)
            return DateTime.UtcNow - StartDate;

        return null;
    }

    public bool IsActive() => Status == TreatmentStatus.InProgress;
    public bool IsCompleted() => Status == TreatmentStatus.Completed;
}