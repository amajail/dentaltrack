namespace DentalTrack.Domain.ValueObjects;

public enum TreatmentStatus
{
    Planned = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4,
    OnHold = 5
}

public static class TreatmentStatusExtensions
{
    public static string GetDisplayName(this TreatmentStatus status)
    {
        return status switch
        {
            TreatmentStatus.Planned => "Planned",
            TreatmentStatus.InProgress => "In Progress",
            TreatmentStatus.Completed => "Completed",
            TreatmentStatus.Cancelled => "Cancelled",
            TreatmentStatus.OnHold => "On Hold",
            _ => "Unknown"
        };
    }

    public static bool IsActive(this TreatmentStatus status)
    {
        return status == TreatmentStatus.InProgress;
    }

    public static bool IsFinished(this TreatmentStatus status)
    {
        return status == TreatmentStatus.Completed || status == TreatmentStatus.Cancelled;
    }
}