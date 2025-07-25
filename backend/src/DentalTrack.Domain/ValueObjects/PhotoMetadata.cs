namespace DentalTrack.Domain.ValueObjects;

public record PhotoMetadata
{
    public int Width { get; }
    public int Height { get; }
    public string? CameraModel { get; }
    public string? CameraMake { get; }
    public DateTime? DateTaken { get; }
    public string? Location { get; }
    public double? ExposureTime { get; }
    public double? FNumber { get; }
    public int? Iso { get; }
    public string? Flash { get; }
    public double? FocalLength { get; }

    public PhotoMetadata(
        int width, 
        int height,
        string? cameraModel = null,
        string? cameraMake = null,
        DateTime? dateTaken = null,
        string? location = null,
        double? exposureTime = null,
        double? fNumber = null,
        int? iso = null,
        string? flash = null,
        double? focalLength = null)
    {
        if (width <= 0)
            throw new ArgumentException("Width must be positive", nameof(width));
        if (height <= 0)
            throw new ArgumentException("Height must be positive", nameof(height));

        Width = width;
        Height = height;
        CameraModel = cameraModel;
        CameraMake = cameraMake;
        DateTaken = dateTaken;
        Location = location;
        ExposureTime = exposureTime;
        FNumber = fNumber;
        Iso = iso;
        Flash = flash;
        FocalLength = focalLength;
    }

    public double GetAspectRatio() => (double)Width / Height;
    
    public long GetTotalPixels() => (long)Width * Height;
    
    public bool IsHighResolution() => GetTotalPixels() >= 2_000_000; // 2MP+
    
    public string GetResolutionDescription()
    {
        var megapixels = GetTotalPixels() / 1_000_000.0;
        return $"{Width}x{Height} ({megapixels:F1}MP)";
    }
}