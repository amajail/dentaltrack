namespace DentalTrack.Domain.ValueObjects;

public enum PhotoQuality
{
    Pending = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Excellent = 4
}

public static class PhotoQualityExtensions
{
    public static string GetDisplayName(this PhotoQuality quality)
    {
        return quality switch
        {
            PhotoQuality.Pending => "Pending Review",
            PhotoQuality.Low => "Low Quality",
            PhotoQuality.Medium => "Medium Quality",
            PhotoQuality.High => "High Quality",
            PhotoQuality.Excellent => "Excellent Quality",
            _ => "Unknown"
        };
    }

    public static bool IsAcceptable(this PhotoQuality quality)
    {
        return quality >= PhotoQuality.Medium;
    }

    public static bool RequiresReview(this PhotoQuality quality)
    {
        return quality == PhotoQuality.Pending || quality == PhotoQuality.Low;
    }
}