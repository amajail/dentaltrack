namespace DentalTrack.Domain.ValueObjects;

public enum AnalysisStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5
}

public static class AnalysisStatusExtensions
{
    public static string GetDisplayName(this AnalysisStatus status)
    {
        return status switch
        {
            AnalysisStatus.Pending => "Pending",
            AnalysisStatus.Processing => "Processing",
            AnalysisStatus.Completed => "Completed",
            AnalysisStatus.Failed => "Failed",
            AnalysisStatus.Cancelled => "Cancelled",
            _ => "Unknown"
        };
    }

    public static bool IsFinished(this AnalysisStatus status)
    {
        return status == AnalysisStatus.Completed ||
               status == AnalysisStatus.Failed ||
               status == AnalysisStatus.Cancelled;
    }

    public static bool CanBeRetried(this AnalysisStatus status)
    {
        return status == AnalysisStatus.Failed;
    }

    public static bool IsActive(this AnalysisStatus status)
    {
        return status == AnalysisStatus.Processing;
    }
}