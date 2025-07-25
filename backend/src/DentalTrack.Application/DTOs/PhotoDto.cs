using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Application.DTOs;

public class PhotoDto
{
    public Guid Id { get; set; }
    public Guid TreatmentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public PhotoType Type { get; set; }
    public string TypeDisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ToothNumber { get; set; }
    public PhotoQuality Quality { get; set; }
    public string QualityDisplayName { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public PhotoMetadataDto Metadata { get; set; } = new();
    public TreatmentDto? Treatment { get; set; }
    public ICollection<AnalysisDto>? Analyses { get; set; }
}

public class PhotoMetadataDto
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string? CameraModel { get; set; }
    public string? CameraMake { get; set; }
    public DateTime? DateTaken { get; set; }
    public string? Location { get; set; }
    public double? ExposureTime { get; set; }
    public double? FNumber { get; set; }
    public int? Iso { get; set; }
    public string? Flash { get; set; }
    public double? FocalLength { get; set; }
    public double AspectRatio { get; set; }
    public long TotalPixels { get; set; }
    public bool IsHighResolution { get; set; }
    public string ResolutionDescription { get; set; } = string.Empty;
}

public class CreatePhotoDto
{
    public Guid TreatmentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public PhotoType Type { get; set; }
    public string? Description { get; set; }
    public int? ToothNumber { get; set; }
    public PhotoMetadataDto Metadata { get; set; } = new();
}

public class UpdatePhotoDto
{
    public string? Description { get; set; }
    public int? ToothNumber { get; set; }
    public PhotoQuality? Quality { get; set; }
}