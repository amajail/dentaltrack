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

public class UpdateTreatmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITreatmentRepository> _mockTreatmentRepository;
    private readonly IMapper _mapper;
    private readonly UpdateTreatmentHandler _handler;

    public UpdateTreatmentHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTreatmentRepository = new Mock<ITreatmentRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _mockUnitOfWork.Setup(x => x.Treatments).Returns(_mockTreatmentRepository.Object);

        _handler = new UpdateTreatmentHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WithValidTreatmentAndData_ShouldUpdateTreatmentAndReturnDto()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Whitening,
            title: "Original Whitening",
            description: "Original description",
            estimatedCost: 500m,
            startDate: DateTime.UtcNow.AddDays(1)
        );

        var updateDto = new UpdateTreatmentDto
        {
            Title = "Updated Whitening Treatment",
            Description = "Updated professional whitening treatment",
            EstimatedCost = 600m
        };

        var command = new UpdateTreatmentCommand(treatmentId, updateDto);

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
        result.Title.Should().Be("Updated Whitening Treatment");
        result.Description.Should().Be("Updated professional whitening treatment");
        result.EstimatedCost.Should().Be(600m);
        result.Type.Should().Be(TreatmentType.Whitening);
        result.PatientId.Should().Be(patientId);
        result.Status.Should().Be(TreatmentStatus.Planned);

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
        var updateDto = new UpdateTreatmentDto
        {
            Title = "Updated Treatment",
            Description = "Updated description",
            EstimatedCost = 400m
        };
        var command = new UpdateTreatmentCommand(treatmentId, updateDto);

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
    public async Task Handle_WithPartialUpdate_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Cleaning,
            title: "Original Cleaning",
            description: "Original cleaning description",
            estimatedCost: 200m,
            startDate: DateTime.UtcNow.AddDays(2)
        );

        var updateDto = new UpdateTreatmentDto
        {
            Title = "Updated Deep Cleaning",
            Description = null,  // Keep original description
            EstimatedCost = null // Keep original cost
        };

        var command = new UpdateTreatmentCommand(treatmentId, updateDto);

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
        result.Title.Should().Be("Updated Deep Cleaning");
        result.Description.Should().BeNull(); // Updated to null
        result.EstimatedCost.Should().BeNull(); // Updated to null
    }

    [Fact]
    public async Task Handle_WithInProgressTreatment_ShouldUpdateSuccessfully()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Orthodontics,
            title: "Braces Treatment",
            description: "Traditional braces",
            estimatedCost: 3000m,
            startDate: DateTime.UtcNow.AddDays(-5)
        );

        // Start the treatment
        treatment.Start();

        var updateDto = new UpdateTreatmentDto
        {
            Title = "Advanced Braces Treatment",
            Description = "Advanced orthodontic treatment with ceramic braces",
            EstimatedCost = 3500m
        };

        var command = new UpdateTreatmentCommand(treatmentId, updateDto);

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
        result.Title.Should().Be("Advanced Braces Treatment");
        result.Description.Should().Be("Advanced orthodontic treatment with ceramic braces");
        result.EstimatedCost.Should().Be(3500m);
        result.Status.Should().Be(TreatmentStatus.InProgress);
        result.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_WithEmptyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Whitening,
            title: "Original Title",
            description: "Original description",
            estimatedCost: 500m,
            startDate: DateTime.UtcNow.AddDays(1)
        );

        var updateDto = new UpdateTreatmentDto
        {
            Title = "", // Empty title should cause validation error
            Description = "Updated description",
            EstimatedCost = 600m
        };

        var command = new UpdateTreatmentCommand(treatmentId, updateDto);

        _mockTreatmentRepository
            .Setup(x => x.GetByIdAsync(treatmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Title cannot be empty*");

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
    public async Task Handle_WithNegativeEstimatedCost_ShouldUpdateSuccessfully()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Whitening,
            title: "Original Title",
            description: "Original description",
            estimatedCost: 500m,
            startDate: DateTime.UtcNow.AddDays(1)
        );

        var updateDto = new UpdateTreatmentDto
        {
            Title = "Updated Title",
            Description = "Updated description",
            EstimatedCost = -100m // The domain allows negative estimated cost
        };

        var command = new UpdateTreatmentCommand(treatmentId, updateDto);

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
        result.Title.Should().Be("Updated Title");
        result.Description.Should().Be("Updated description");
        result.EstimatedCost.Should().Be(-100m);

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
    public async Task Handle_ShouldMapTreatmentCorrectly()
    {
        // Arrange
        var treatmentId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var treatment = new Treatment(
            patientId: patientId,
            type: TreatmentType.Filling,
            title: "Cavity Filling",
            description: "Fill cavity in tooth 12",
            estimatedCost: 150m,
            startDate: DateTime.UtcNow.AddDays(3)
        );

        var updateDto = new UpdateTreatmentDto
        {
            Title = "Composite Filling",
            Description = "Composite resin filling for tooth 12",
            EstimatedCost = 180m
        };

        var command = new UpdateTreatmentCommand(treatmentId, updateDto);

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
        result.Id.Should().NotBeEmpty();
        result.PatientId.Should().Be(patientId);
        result.Type.Should().Be(TreatmentType.Filling);
        result.Title.Should().Be("Composite Filling");
        result.Description.Should().Be("Composite resin filling for tooth 12");
        result.EstimatedCost.Should().Be(180m);
        result.Status.Should().Be(TreatmentStatus.Planned);
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}