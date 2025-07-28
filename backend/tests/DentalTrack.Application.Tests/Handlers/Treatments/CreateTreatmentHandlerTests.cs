using AutoMapper;
using DentalTrack.Application.Commands.Treatments;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Handlers.Treatments;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Domain.ValueObjects;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Treatments;

public class CreateTreatmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateTreatmentHandler _handler;

    public CreateTreatmentHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateTreatmentHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_CreatesAndReturnsTreatment()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var treatmentId = Guid.NewGuid();

        var createTreatmentDto = new CreateTreatmentDto
        {
            PatientId = patientId,
            Type = TreatmentType.Cleaning,
            Title = "Regular Cleaning",
            Description = "Routine dental cleaning",
            EstimatedCost = 150.00m,
            StartDate = DateTime.UtcNow
        };

        var command = new CreateTreatmentCommand(createTreatmentDto);

        var patient = new Patient("John", "Doe", "john.doe@email.com", DateTime.Now.AddYears(-30), "123-456-7890");

        var treatment = new Treatment(
            patientId,
            TreatmentType.Cleaning,
            "Regular Cleaning",
            "Routine dental cleaning",
            150.00m);

        var treatmentDto = new TreatmentDto
        {
            Id = treatmentId,
            PatientId = patientId,
            Type = TreatmentType.Cleaning,
            Title = "Regular Cleaning",
            Description = "Routine dental cleaning",
            EstimatedCost = 150.00m,
            Status = TreatmentStatus.Planned
        };

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        _mapperMock.Setup(x => x.Map<Treatment>(createTreatmentDto))
            .Returns(treatment);

        _mapperMock.Setup(x => x.Map<TreatmentDto>(treatment))
            .Returns(treatmentDto);

        _unitOfWorkMock.Setup(x => x.Treatments.AddAsync(treatment, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(treatmentDto.Id, result.Id);
        Assert.Equal(treatmentDto.PatientId, result.PatientId);
        Assert.Equal(treatmentDto.Type, result.Type);
        Assert.Equal(treatmentDto.Title, result.Title);
        Assert.Equal(treatmentDto.Description, result.Description);
        Assert.Equal(treatmentDto.EstimatedCost, result.EstimatedCost);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Treatments.AddAsync(treatment, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<Treatment>(createTreatmentDto), Times.Once);
        _mapperMock.Verify(x => x.Map<TreatmentDto>(treatment), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        var createTreatmentDto = new CreateTreatmentDto
        {
            PatientId = patientId,
            Type = TreatmentType.Cleaning,
            Title = "Regular Cleaning"
        };

        var command = new CreateTreatmentCommand(createTreatmentDto);

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains($"Patient with ID {patientId} not found", exception.Message);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Treatments.AddAsync(It.IsAny<Treatment>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

}