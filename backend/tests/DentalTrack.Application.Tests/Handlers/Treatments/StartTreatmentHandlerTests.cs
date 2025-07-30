using AutoMapper;
using DentalTrack.Application.Commands.Treatments;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Handlers.Treatments;
using DentalTrack.Application.Mappings;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Treatments;

public class StartTreatmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITreatmentRepository> _mockTreatmentRepository;
    private readonly IMapper _mapper;
    private readonly StartTreatmentHandler _handler;

    public StartTreatmentHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTreatmentRepository = new Mock<ITreatmentRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _mockUnitOfWork.Setup(x => x.Treatments).Returns(_mockTreatmentRepository.Object);

        _handler = new StartTreatmentHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WithValidTreatmentId_ShouldStartTreatmentAndReturnDto()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Whitening,
            title: "Teeth Whitening",
            description: "Professional whitening treatment",
            estimatedCost: 500m,
            startDate: DateTime.UtcNow.AddDays(1)
        );

        var command = new StartTreatmentCommand(treatmentId);

        _mockTreatmentRepository
            .Setup(x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        _mockTreatmentRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Treatment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(TreatmentStatus.InProgress);
        result.Title.Should().Be("Teeth Whitening");
        result.Type.Should().Be(TreatmentType.Whitening);
        result.PatientId.Should().Be(patientId);

        _mockTreatmentRepository.Verify(
            x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockTreatmentRepository.Verify(
            x => x.UpdateAsync(It.IsAny<Treatment>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentTreatment_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var command = new StartTreatmentCommand(treatmentId);

        _mockTreatmentRepository
            .Setup(x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Treatment?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Treatment with ID {treatmentId} not found");

        _mockTreatmentRepository.Verify(
            x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockTreatmentRepository.Verify(
            x => x.UpdateAsync(It.IsAny<Treatment>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithTreatmentAlreadyStarted_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Whitening,
            title: "Teeth Whitening",
            description: "Professional whitening treatment",
            estimatedCost: 500m,
            startDate: DateTime.UtcNow.AddDays(1)
        );

        // Start the treatment first
        treatment.Start();

        var command = new StartTreatmentCommand(treatmentId);

        _mockTreatmentRepository
            .Setup(x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Treatment can only be started from Planned status");

        _mockTreatmentRepository.Verify(
            x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockTreatmentRepository.Verify(
            x => x.UpdateAsync(It.IsAny<Treatment>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldMapTreatmentCorrectly()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Cleaning,
            title: "Deep Cleaning",
            description: "Professional deep cleaning",
            estimatedCost: 200m,
            startDate: DateTime.UtcNow.AddDays(2)
        );

        var command = new StartTreatmentCommand(treatmentId);

        _mockTreatmentRepository
            .Setup(x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        _mockTreatmentRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Treatment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(TreatmentStatus.InProgress);
        result.Title.Should().Be("Deep Cleaning");
        result.Description.Should().Be("Professional deep cleaning");
        result.Type.Should().Be(TreatmentType.Cleaning);
        result.EstimatedCost.Should().Be(200m);
        result.PatientId.Should().Be(patientId);
        result.Id.Should().NotBeEmpty();
        result.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}