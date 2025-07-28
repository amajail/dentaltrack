using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Handlers.Treatments;
using DentalTrack.Application.Mappings;
using DentalTrack.Application.Queries.Treatments;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Treatments;

public class GetTreatmentsByPatientHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITreatmentRepository> _mockTreatmentRepository;
    private readonly IMapper _mapper;
    private readonly GetTreatmentsByPatientHandler _handler;

    public GetTreatmentsByPatientHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTreatmentRepository = new Mock<ITreatmentRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _mockUnitOfWork.Setup(x => x.Treatments).Returns(_mockTreatmentRepository.Object);

        _handler = new GetTreatmentsByPatientHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WithExistingTreatments_ShouldReturnTreatmentDtos()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        var treatments = new List<Treatment>
        {
            new(patientId, TreatmentType.Cleaning, "Routine Cleaning", "Regular dental cleaning", 150.00m, DateTime.UtcNow),
            new(patientId, TreatmentType.Filling, "Cavity Filling", "Fill cavity on tooth #12", 300.00m, DateTime.UtcNow.AddDays(1))
        };

        // Set the Patient property for mapping
        treatments[0].GetType().GetProperty("Patient")?.SetValue(treatments[0], patient);
        treatments[1].GetType().GetProperty("Patient")?.SetValue(treatments[1], patient);

        var query = new GetTreatmentsByPatientQuery(patientId);

        _mockTreatmentRepository
            .Setup(x => x.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatments);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var resultList = result.ToList();
        resultList[0].PatientId.Should().Be(patientId);
        resultList[0].Type.Should().Be(TreatmentType.Cleaning);
        resultList[0].Title.Should().Be("Routine Cleaning");

        resultList[1].PatientId.Should().Be(patientId);
        resultList[1].Type.Should().Be(TreatmentType.Filling);
        resultList[1].Title.Should().Be("Cavity Filling");

        _mockTreatmentRepository.Verify(
            x => x.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoTreatments_ShouldReturnEmptyCollection()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var treatments = new List<Treatment>();
        var query = new GetTreatmentsByPatientQuery(patientId);

        _mockTreatmentRepository
            .Setup(x => x.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatments);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockTreatmentRepository.Verify(
            x => x.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapTreatmentsCorrectly()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var patient = new Patient("Test", "Patient", "test@example.com", new DateTime(1980, 6, 15));
        var treatment = new Treatment(
            patientId,
            TreatmentType.RootCanal,
            "Root Canal Treatment",
            "Root canal on tooth #8",
            1200.00m,
            DateTime.UtcNow);

        // Set the Patient property for mapping
        treatment.GetType().GetProperty("Patient")?.SetValue(treatment, patient);

        var treatments = new List<Treatment> { treatment };
        var query = new GetTreatmentsByPatientQuery(patientId);

        _mockTreatmentRepository
            .Setup(x => x.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatments);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var treatmentDto = result.First();
        treatmentDto.Should().NotBeNull();
        treatmentDto.Id.Should().Be(treatment.Id);
        treatmentDto.PatientId.Should().Be(treatment.PatientId);
        treatmentDto.Type.Should().Be(treatment.Type);
        treatmentDto.Title.Should().Be(treatment.Title);
        treatmentDto.Description.Should().Be(treatment.Description);
        treatmentDto.EstimatedCost.Should().Be(treatment.EstimatedCost);
        treatmentDto.StartDate.Should().Be(treatment.StartDate);
        treatmentDto.Status.Should().Be(treatment.Status);
        // PatientName should be mapped from the Patient navigation property
        // Note: This depends on the AutoMapper configuration
    }
}