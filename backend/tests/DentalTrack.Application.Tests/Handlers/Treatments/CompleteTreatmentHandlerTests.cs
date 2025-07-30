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

public class CompleteTreatmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITreatmentRepository> _mockTreatmentRepository;
    private readonly IMapper _mapper;
    private readonly CompleteTreatmentHandler _handler;

    public CompleteTreatmentHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTreatmentRepository = new Mock<ITreatmentRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _mockUnitOfWork.Setup(x => x.Treatments).Returns(_mockTreatmentRepository.Object);

        _handler = new CompleteTreatmentHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WithValidInProgressTreatment_ShouldCompleteTreatmentAndReturnDto()
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
            startDate: DateTime.UtcNow.AddDays(-1)
        );

        // Start the treatment so it can be completed
        treatment.Start();

        var completionDto = new CompleteTreatmentDto
        {
            ActualCost = 450m,
            Notes = "Treatment completed successfully"
        };

        var command = new CompleteTreatmentCommand(treatmentId, completionDto);

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
        result.Status.Should().Be(TreatmentStatus.Completed);
        result.Title.Should().Be("Teeth Whitening");
        result.Type.Should().Be(TreatmentType.Whitening);
        result.PatientId.Should().Be(patientId);
        result.ActualCost.Should().Be(450m);
        result.Notes.Should().Be("Treatment completed successfully");
        result.EndDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

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
        var completionDto = new CompleteTreatmentDto
        {
            ActualCost = 450m,
            Notes = "Treatment completed successfully"
        };
        var command = new CompleteTreatmentCommand(treatmentId, completionDto);

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
    public async Task Handle_WithTreatmentNotInProgress_ShouldThrowInvalidOperationException()
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
        // Don't start the treatment - it remains in Planned status

        var completionDto = new CompleteTreatmentDto
        {
            ActualCost = 450m,
            Notes = "Treatment completed successfully"
        };

        var command = new CompleteTreatmentCommand(treatmentId, completionDto);

        _mockTreatmentRepository
            .Setup(x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Treatment can only be completed from InProgress status");

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
    public async Task Handle_WithAlreadyCompletedTreatment_ShouldThrowInvalidOperationException()
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
            startDate: DateTime.UtcNow.AddDays(-2)
        );

        // Start and complete the treatment
        treatment.Start();
        treatment.Complete(400m, "Already completed");

        var completionDto = new CompleteTreatmentDto
        {
            ActualCost = 450m,
            Notes = "Trying to complete again"
        };

        var command = new CompleteTreatmentCommand(treatmentId, completionDto);

        _mockTreatmentRepository
            .Setup(x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Treatment can only be completed from InProgress status");

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
    public async Task Handle_WithMinimalCompletionData_ShouldCompleteSuccessfully()
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
            startDate: DateTime.UtcNow.AddDays(-1)
        );

        treatment.Start();

        var completionDto = new CompleteTreatmentDto
        {
            ActualCost = null, // No actual cost provided
            Notes = null       // No notes provided
        };

        var command = new CompleteTreatmentCommand(treatmentId, completionDto);

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
        result.Status.Should().Be(TreatmentStatus.Completed);
        result.ActualCost.Should().BeNull();
        result.Notes.Should().BeNull();
        result.EndDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_ShouldMapTreatmentCorrectly()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Orthodontics,
            title: "Braces Treatment",
            description: "Orthodontic treatment with braces",
            estimatedCost: 3000m,
            startDate: DateTime.UtcNow.AddDays(-30)
        );

        treatment.Start();

        var completionDto = new CompleteTreatmentDto
        {
            ActualCost = 2800m,
            Notes = "Excellent results achieved"
        };

        var command = new CompleteTreatmentCommand(treatmentId, completionDto);

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
        result.Status.Should().Be(TreatmentStatus.Completed);
        result.Title.Should().Be("Braces Treatment");
        result.Description.Should().Be("Orthodontic treatment with braces");
        result.Type.Should().Be(TreatmentType.Orthodontics);
        result.EstimatedCost.Should().Be(3000m);
        result.ActualCost.Should().Be(2800m);
        result.Notes.Should().Be("Excellent results achieved");
        result.PatientId.Should().Be(patientId);
        result.Id.Should().NotBeEmpty();
    }
}