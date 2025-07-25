using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Tests.Entities;

public class PhotoTests
{
    private readonly Guid _treatmentId = Guid.NewGuid();
    private readonly string _fileName = "test.jpg";
    private readonly string _filePath = "/uploads/test.jpg";
    private readonly string _contentType = "image/jpeg";
    private readonly long _fileSize = 1024000;
    private readonly PhotoType _photoType = PhotoType.Before;
    private readonly PhotoMetadata _metadata = new(1920, 1080);

    [Fact]
    public void Constructor_WithValidInputs_CreatesPhoto()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);

        Assert.Equal(_treatmentId, photo.TreatmentId);
        Assert.Equal(_fileName, photo.FileName);
        Assert.Equal(_filePath, photo.FilePath);
        Assert.Equal(_contentType, photo.ContentType);
        Assert.Equal(_fileSize, photo.FileSize);
        Assert.Equal(_photoType, photo.Type);
        Assert.Equal(_metadata, photo.Metadata);
        Assert.Null(photo.Description);
        Assert.Null(photo.ToothNumber);
        Assert.Equal(PhotoQuality.Pending, photo.Quality);
        Assert.False(photo.IsProcessed);
        Assert.Empty(photo.Analyses);
    }

    [Fact]
    public void Constructor_WithOptionalParameters_CreatesPhotoWithValues()
    {
        var description = "Test description";
        var toothNumber = 12;

        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata, description, toothNumber);

        Assert.Equal(description, photo.Description);
        Assert.Equal(toothNumber, photo.ToothNumber);
    }

    [Fact]
    public void Constructor_WithEmptyTreatmentId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => 
            new Photo(Guid.Empty, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidFileName_ThrowsArgumentException(string? invalidFileName)
    {
        Assert.Throws<ArgumentException>(() => 
            new Photo(_treatmentId, invalidFileName!, _filePath, _contentType, _fileSize, _photoType, _metadata));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidFilePath_ThrowsArgumentException(string? invalidFilePath)
    {
        Assert.Throws<ArgumentException>(() => 
            new Photo(_treatmentId, _fileName, invalidFilePath!, _contentType, _fileSize, _photoType, _metadata));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidContentType_ThrowsArgumentException(string? invalidContentType)
    {
        Assert.Throws<ArgumentException>(() => 
            new Photo(_treatmentId, _fileName, _filePath, invalidContentType!, _fileSize, _photoType, _metadata));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void Constructor_WithInvalidFileSize_ThrowsArgumentException(long invalidFileSize)
    {
        Assert.Throws<ArgumentException>(() => 
            new Photo(_treatmentId, _fileName, _filePath, _contentType, invalidFileSize, _photoType, _metadata));
    }

    [Fact]
    public void UpdateDescription_WithValidDescription_UpdatesDescription()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);
        var newDescription = "Updated description";

        photo.UpdateDescription(newDescription);

        Assert.Equal(newDescription, photo.Description);
    }

    [Fact]
    public void UpdateDescription_WithNull_ClearsDescription()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata, "Initial description");

        photo.UpdateDescription(null);

        Assert.Null(photo.Description);
    }

    [Fact]
    public void SetQuality_WithValidQuality_UpdatesQuality()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);

        photo.SetQuality(PhotoQuality.High);

        Assert.Equal(PhotoQuality.High, photo.Quality);
    }

    [Fact]
    public void MarkAsProcessed_SetsIsProcessedToTrue()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);

        photo.MarkAsProcessed();

        Assert.True(photo.IsProcessed);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(16)]
    [InlineData(32)]
    public void UpdateToothNumber_WithValidToothNumber_UpdatesToothNumber(int validToothNumber)
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);

        photo.UpdateToothNumber(validToothNumber);

        Assert.Equal(validToothNumber, photo.ToothNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(33)]
    [InlineData(100)]
    public void UpdateToothNumber_WithInvalidToothNumber_ThrowsArgumentException(int invalidToothNumber)
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);

        Assert.Throws<ArgumentException>(() => photo.UpdateToothNumber(invalidToothNumber));
    }

    [Fact]
    public void UpdateToothNumber_WithNull_ClearsToothNumber()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata, null, 12);

        photo.UpdateToothNumber(null);

        Assert.Null(photo.ToothNumber);
    }

    [Fact]
    public void IsHighQuality_WithHighQuality_ReturnsTrue()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);
        photo.SetQuality(PhotoQuality.High);

        Assert.True(photo.IsHighQuality());
    }

    [Theory]
    [InlineData(PhotoQuality.Pending)]
    [InlineData(PhotoQuality.Low)]
    [InlineData(PhotoQuality.Medium)]
    public void IsHighQuality_WithNonHighQuality_ReturnsFalse(PhotoQuality quality)
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);
        photo.SetQuality(quality);

        Assert.False(photo.IsHighQuality());
    }

    [Fact]
    public void RequiresReview_WithLowQuality_ReturnsTrue()
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);
        photo.SetQuality(PhotoQuality.Low);

        Assert.True(photo.RequiresReview());
    }

    [Theory]
    [InlineData(PhotoQuality.Pending)]
    [InlineData(PhotoQuality.Medium)]
    [InlineData(PhotoQuality.High)]
    public void RequiresReview_WithNonLowQuality_ReturnsFalse(PhotoQuality quality)
    {
        var photo = new Photo(_treatmentId, _fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);
        photo.SetQuality(quality);

        Assert.False(photo.RequiresReview());
    }

    [Theory]
    [InlineData("test.jpg", ".jpg")]
    [InlineData("image.PNG", ".png")]
    [InlineData("photo.JPEG", ".jpeg")]
    [InlineData("file.gif", ".gif")]
    [InlineData("noextension", "")]
    public void GetFileExtension_ReturnsLowerCaseExtension(string fileName, string expectedExtension)
    {
        var photo = new Photo(_treatmentId, fileName, _filePath, _contentType, _fileSize, _photoType, _metadata);

        Assert.Equal(expectedExtension, photo.GetFileExtension());
    }
}