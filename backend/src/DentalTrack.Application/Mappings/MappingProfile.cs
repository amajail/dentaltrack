using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.GetFullName()))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.GetAge()));

        CreateMap<CreatePatientDto, Patient>()
            .ConstructUsing(src => new Patient(
                src.FirstName,
                src.LastName,
                src.Email,
                src.DateOfBirth,
                src.Phone,
                src.Gender,
                src.Address,
                src.EmergencyContact,
                src.EmergencyPhone,
                src.MedicalHistory,
                src.Allergies))
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Treatments, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Treatment, TreatmentDto>()
            .ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => src.Status.GetDisplayName()))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.GetDuration()))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive()))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted()));

        CreateMap<CreateTreatmentDto, Treatment>()
            .ConstructUsing(src => new Treatment(
                src.PatientId,
                src.Type,
                src.Title,
                src.Description,
                src.EstimatedCost,
                src.StartDate))
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.EndDate, opt => opt.Ignore())
            .ForMember(dest => dest.ActualCost, opt => opt.Ignore())
            .ForMember(dest => dest.Notes, opt => opt.Ignore())
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .ForMember(dest => dest.Analyses, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Photo, PhotoDto>()
            .ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ForMember(dest => dest.QualityDisplayName, opt => opt.MapFrom(src => src.Quality.GetDisplayName()));

        CreateMap<PhotoMetadata, PhotoMetadataDto>()
            .ForMember(dest => dest.AspectRatio, opt => opt.MapFrom(src => src.GetAspectRatio()))
            .ForMember(dest => dest.TotalPixels, opt => opt.MapFrom(src => src.GetTotalPixels()))
            .ForMember(dest => dest.IsHighResolution, opt => opt.MapFrom(src => src.IsHighResolution()))
            .ForMember(dest => dest.ResolutionDescription, opt => opt.MapFrom(src => src.GetResolutionDescription()));

        CreateMap<PhotoMetadataDto, PhotoMetadata>()
            .ConstructUsing(src => new PhotoMetadata(
                src.Width,
                src.Height,
                src.CameraModel,
                src.CameraMake,
                src.DateTaken,
                src.Location,
                src.ExposureTime,
                src.FNumber,
                src.Iso,
                src.Flash,
                src.FocalLength));

        CreateMap<CreatePhotoDto, Photo>()
            .ConstructUsing(src => new Photo(
                src.TreatmentId,
                src.FileName,
                src.FilePath,
                src.ContentType,
                src.FileSize,
                src.Type,
                new PhotoMetadata(
                    src.Metadata.Width,
                    src.Metadata.Height,
                    src.Metadata.CameraModel,
                    src.Metadata.CameraMake,
                    src.Metadata.DateTaken,
                    src.Metadata.Location,
                    src.Metadata.ExposureTime,
                    src.Metadata.FNumber,
                    src.Metadata.Iso,
                    src.Metadata.Flash,
                    src.Metadata.FocalLength),
                src.Description,
                src.ToothNumber))
            .ForMember(dest => dest.Treatment, opt => opt.Ignore())
            .ForMember(dest => dest.Quality, opt => opt.Ignore())
            .ForMember(dest => dest.IsProcessed, opt => opt.Ignore())
            .ForMember(dest => dest.Analyses, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Analysis, AnalysisDto>()
            .ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ForMember(dest => dest.TypeDescription, opt => opt.MapFrom(src => src.Type.GetDescription()))
            .ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => src.Status.GetDisplayName()))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted()))
            .ForMember(dest => dest.HasFailed, opt => opt.MapFrom(src => src.HasFailed()))
            .ForMember(dest => dest.IsProcessing, opt => opt.MapFrom(src => src.IsProcessing()))
            .ForMember(dest => dest.HasHighConfidence, opt => opt.MapFrom(src => src.HasHighConfidence()))
            .ForMember(dest => dest.IsAIBased, opt => opt.MapFrom(src => src.Type.IsAIBased()));

        CreateMap<CreateAnalysisDto, Analysis>()
            .ConstructUsing(src => new Analysis(src.PhotoId, src.Type))
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Results, opt => opt.Ignore())
            .ForMember(dest => dest.ConfidenceScore, opt => opt.Ignore())
            .ForMember(dest => dest.Findings, opt => opt.Ignore())
            .ForMember(dest => dest.Recommendations, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ErrorMessage, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessingTimeMs, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}