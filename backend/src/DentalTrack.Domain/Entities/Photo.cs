using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Entities;

public class Photo : BaseEntity
{
    public Guid TreatmentId { get; private set; }
    public Treatment Treatment { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public string ContentType { get; private set; }
    public long FileSize { get; private set; }
    public PhotoMetadata Metadata { get; private set; }
    public PhotoType Type { get; private set; }
    public string? Description { get; private set; }
    public int? ToothNumber { get; private set; }
    public PhotoQuality Quality { get; private set; }
    public bool IsProcessed { get; private set; }

    public ICollection<Analysis> Analyses { get; private set; } = new List<Analysis>();

    private Photo() { }

    public Photo(
        Guid treatmentId,
        string fileName,
        string filePath,
        string contentType,
        long fileSize,
        PhotoType type,
        PhotoMetadata metadata,
        string? description = null,
        int? toothNumber = null)
    {
        if (treatmentId == Guid.Empty)
            throw new ArgumentException("Treatment ID cannot be empty", nameof(treatmentId));
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty", nameof(filePath));
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type cannot be empty", nameof(contentType));
        if (fileSize <= 0)
            throw new ArgumentException("File size must be positive", nameof(fileSize));

        TreatmentId = treatmentId;
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        FileSize = fileSize;
        Type = type;
        Metadata = metadata;
        Description = description;
        ToothNumber = toothNumber;
        Quality = PhotoQuality.Pending;
        IsProcessed = false;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        SetUpdatedAt();
    }

    public void SetQuality(PhotoQuality quality)
    {
        Quality = quality;
        SetUpdatedAt();
    }

    public void MarkAsProcessed()
    {
        IsProcessed = true;
        SetUpdatedAt();
    }

    public void UpdateToothNumber(int? toothNumber)
    {
        if (toothNumber.HasValue && (toothNumber < 1 || toothNumber > 32))
            throw new ArgumentException("Tooth number must be between 1 and 32", nameof(toothNumber));

        ToothNumber = toothNumber;
        SetUpdatedAt();
    }

    public bool IsHighQuality() => Quality == PhotoQuality.High;
    public bool RequiresReview() => Quality == PhotoQuality.Low;
    public string GetFileExtension() => Path.GetExtension(FileName).ToLowerInvariant();
}