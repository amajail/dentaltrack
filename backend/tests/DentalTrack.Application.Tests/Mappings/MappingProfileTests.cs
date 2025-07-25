using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Mappings;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Application.Tests.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        // Act & Assert
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Patient_ToPatientDto_ShouldMapCorrectly()
    {
        // Arrange
        var patient = new Patient(
            "John",
            "Doe", 
            "john@example.com",
            new DateTime(1985, 5, 15),
            "555-1234",
            "123 Main St",
            "Emergency Contact",
            "555-9999",
            "Medical history",
            "Allergies");

        // Act
        var result = _mapper.Map<PatientDto>(patient);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(patient.Id);
        result.FirstName.Should().Be(patient.FirstName);
        result.LastName.Should().Be(patient.LastName);
        result.Email.Should().Be(patient.Email);
        result.Phone.Should().Be(patient.Phone);
        result.Address.Should().Be(patient.Address);
        result.EmergencyContact.Should().Be(patient.EmergencyContact);
        result.EmergencyPhone.Should().Be(patient.EmergencyPhone);
        result.MedicalHistory.Should().Be(patient.MedicalHistory);
        result.Allergies.Should().Be(patient.Allergies);
        result.IsActive.Should().Be(patient.IsActive);
        result.FullName.Should().Be(patient.GetFullName());
        result.Age.Should().Be(patient.GetAge());
        result.CreatedAt.Should().Be(patient.CreatedAt);
        result.UpdatedAt.Should().Be(patient.UpdatedAt);
    }

    [Fact]
    public void CreatePatientDto_ToPatient_ShouldMapCorrectly()
    {
        // Arrange
        var createDto = new CreatePatientDto
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            DateOfBirth = new DateTime(1990, 3, 20),
            Phone = "555-5678",
            Address = "456 Oak Ave",
            EmergencyContact = "Emergency",
            EmergencyPhone = "555-0000",
            MedicalHistory = "History",
            Allergies = "Allergies"
        };

        // Act
        var result = _mapper.Map<Patient>(createDto);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(createDto.FirstName);
        result.LastName.Should().Be(createDto.LastName);
        result.Email.Should().Be(createDto.Email);
        result.DateOfBirth.Should().Be(createDto.DateOfBirth);
        result.Phone.Should().Be(createDto.Phone);
        result.Address.Should().Be(createDto.Address);
        result.EmergencyContact.Should().Be(createDto.EmergencyContact);
        result.EmergencyPhone.Should().Be(createDto.EmergencyPhone);
        result.MedicalHistory.Should().Be(createDto.MedicalHistory);
        result.Allergies.Should().Be(createDto.Allergies);
        result.IsActive.Should().BeTrue();
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Treatment_ToTreatmentDto_ShouldMapCorrectly()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId,
            TreatmentType.Filling,
            "Dental Filling",
            "Description",
            150.00m);

        treatment.Start();

        // Act
        var result = _mapper.Map<TreatmentDto>(treatment);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(treatment.Id);
        result.PatientId.Should().Be(treatment.PatientId);
        result.Type.Should().Be(treatment.Type);
        result.TypeDisplayName.Should().Be(treatment.Type.GetDisplayName());
        result.Title.Should().Be(treatment.Title);
        result.Description.Should().Be(treatment.Description);
        result.Status.Should().Be(treatment.Status);
        result.StatusDisplayName.Should().Be(treatment.Status.GetDisplayName());
        result.StartDate.Should().Be(treatment.StartDate);
        result.EstimatedCost.Should().Be(treatment.EstimatedCost);
        result.IsActive.Should().Be(treatment.IsActive());
        result.IsCompleted.Should().Be(treatment.IsCompleted());
        result.Duration.Should().NotBeNull();
        result.Duration.Should().BeCloseTo(treatment.GetDuration().Value, TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    public void CreateTreatmentDto_ToTreatment_ShouldMapCorrectly()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var createDto = new CreateTreatmentDto
        {
            PatientId = patientId,
            Type = TreatmentType.Crown,
            Title = "Crown Procedure",
            Description = "Crown description",
            EstimatedCost = 800.00m,
            StartDate = new DateTime(2024, 6, 1)
        };

        // Act
        var result = _mapper.Map<Treatment>(createDto);

        // Assert
        result.Should().NotBeNull();
        result.PatientId.Should().Be(createDto.PatientId);
        result.Type.Should().Be(createDto.Type);
        result.Title.Should().Be(createDto.Title);
        result.Description.Should().Be(createDto.Description);
        result.EstimatedCost.Should().Be(createDto.EstimatedCost);
        result.StartDate.Should().Be(createDto.StartDate.Value);
        result.Status.Should().Be(TreatmentStatus.Planned);
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void PhotoMetadata_ToPhotoMetadataDto_ShouldMapCorrectly()
    {
        // Arrange
        var metadata = new PhotoMetadata(
            1920, 
            1080, 
            "Canon EOS R5", 
            "Canon",
            new DateTime(2024, 1, 15),
            "Clinic",
            0.125,
            2.8,
            800,
            "Auto",
            85.0);

        // Act
        var result = _mapper.Map<PhotoMetadataDto>(metadata);

        // Assert
        result.Should().NotBeNull();
        result.Width.Should().Be(metadata.Width);
        result.Height.Should().Be(metadata.Height);
        result.CameraModel.Should().Be(metadata.CameraModel);
        result.CameraMake.Should().Be(metadata.CameraMake);
        result.DateTaken.Should().Be(metadata.DateTaken);
        result.Location.Should().Be(metadata.Location);
        result.ExposureTime.Should().Be(metadata.ExposureTime);
        result.FNumber.Should().Be(metadata.FNumber);
        result.Iso.Should().Be(metadata.Iso);
        result.Flash.Should().Be(metadata.Flash);
        result.FocalLength.Should().Be(metadata.FocalLength);
        result.AspectRatio.Should().Be(metadata.GetAspectRatio());
        result.TotalPixels.Should().Be(metadata.GetTotalPixels());
        result.IsHighResolution.Should().Be(metadata.IsHighResolution());
        result.ResolutionDescription.Should().Be(metadata.GetResolutionDescription());
    }

    [Fact]
    public void PhotoMetadataDto_ToPhotoMetadata_ShouldMapCorrectly()
    {
        // Arrange
        var dto = new PhotoMetadataDto
        {
            Width = 1920,
            Height = 1080,
            CameraModel = "Canon EOS R5",
            CameraMake = "Canon",
            DateTaken = new DateTime(2024, 1, 15),
            Location = "Clinic",
            ExposureTime = 0.125,
            FNumber = 2.8,
            Iso = 800,
            Flash = "Auto",
            FocalLength = 85.0
        };

        // Act
        var result = _mapper.Map<PhotoMetadata>(dto);

        // Assert
        result.Should().NotBeNull();
        result.Width.Should().Be(dto.Width);
        result.Height.Should().Be(dto.Height);
        result.CameraModel.Should().Be(dto.CameraModel);
        result.CameraMake.Should().Be(dto.CameraMake);
        result.DateTaken.Should().Be(dto.DateTaken);
        result.Location.Should().Be(dto.Location);
        result.ExposureTime.Should().Be(dto.ExposureTime);
        result.FNumber.Should().Be(dto.FNumber);
        result.Iso.Should().Be(dto.Iso);
        result.Flash.Should().Be(dto.Flash);
        result.FocalLength.Should().Be(dto.FocalLength);
    }

    [Fact]
    public void Analysis_ToAnalysisDto_ShouldMapCorrectly()
    {
        // Arrange
        var photoId = Guid.NewGuid();
        var analysis = new Analysis(photoId, AnalysisType.CariesDetection);
        analysis.StartProcessing();
        analysis.Complete("Test results", 0.95m, "Test findings", "Test recommendations", 5000);

        // Act
        var result = _mapper.Map<AnalysisDto>(analysis);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(analysis.Id);
        result.PhotoId.Should().Be(analysis.PhotoId);
        result.Type.Should().Be(analysis.Type);
        result.TypeDisplayName.Should().Be(analysis.Type.GetDisplayName());
        result.TypeDescription.Should().Be(analysis.Type.GetDescription());
        result.Status.Should().Be(analysis.Status);
        result.StatusDisplayName.Should().Be(analysis.Status.GetDisplayName());
        result.Results.Should().Be(analysis.Results);
        result.ConfidenceScore.Should().Be(analysis.ConfidenceScore);
        result.Findings.Should().Be(analysis.Findings);
        result.Recommendations.Should().Be(analysis.Recommendations);
        result.CompletedAt.Should().Be(analysis.CompletedAt);
        result.ProcessingTimeMs.Should().Be(analysis.ProcessingTimeMs);
        result.IsCompleted.Should().Be(analysis.IsCompleted());
        result.HasFailed.Should().Be(analysis.HasFailed());
        result.IsProcessing.Should().Be(analysis.IsProcessing());
        result.HasHighConfidence.Should().Be(analysis.HasHighConfidence());
        result.IsAIBased.Should().Be(analysis.Type.IsAIBased());
    }

    [Fact]
    public void CreateAnalysisDto_ToAnalysis_ShouldMapCorrectly()
    {
        // Arrange
        var photoId = Guid.NewGuid();
        var createDto = new CreateAnalysisDto
        {
            PhotoId = photoId,
            Type = AnalysisType.PlaqueAnalysis
        };

        // Act
        var result = _mapper.Map<Analysis>(createDto);

        // Assert
        result.Should().NotBeNull();
        result.PhotoId.Should().Be(createDto.PhotoId);
        result.Type.Should().Be(createDto.Type);
        result.Status.Should().Be(AnalysisStatus.Pending);
        result.Id.Should().NotBeEmpty();
    }
}