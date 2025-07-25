using DentalTrack.Application.Commands.Patients;
using DentalTrack.Application.Handlers.Patients;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Domain.ValueObjects;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Patients;

public class DeletePatientHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeletePatientHandler _handler;

    public DeletePatientHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeletePatientHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidPatientAndNoActiveTreatments_DeactivatesPatientAndReturnsTrue()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var command = new DeletePatientCommand(patientId);

        var patient = new Patient("John", "Doe", "john.doe@email.com", DateTime.Now.AddYears(-30), "123-456-7890");
        var treatments = new List<Treatment>(); // No treatments

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
        
        _unitOfWorkMock.Setup(x => x.Treatments.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatments);

        _unitOfWorkMock.Setup(x => x.Patients.UpdateAsync(patient, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Treatments.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.UpdateAsync(patient, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentPatient_ReturnsFalse()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var command = new DeletePatientCommand(patientId);

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Treatments.GetByPatientIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Patients.UpdateAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithActiveTreatments_ThrowsInvalidOperationException()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var command = new DeletePatientCommand(patientId);

        var patient = new Patient("John", "Doe", "john.doe@email.com", DateTime.Now.AddYears(-30), "123-456-7890");
        
        var activeTreatment = new Treatment(
            patientId,
            TreatmentType.Cleaning,
            "Active Treatment",
            "Description",
            100.00m);
        activeTreatment.Start(); // Make it active

        var treatments = new List<Treatment> { activeTreatment };

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
        
        _unitOfWorkMock.Setup(x => x.Treatments.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatments);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        Assert.Contains("Cannot delete patient with active treatments", exception.Message);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Treatments.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.UpdateAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInactiveTreatments_DeactivatesPatientAndReturnsTrue()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var command = new DeletePatientCommand(patientId);

        var patient = new Patient("John", "Doe", "john.doe@email.com", DateTime.Now.AddYears(-30), "123-456-7890");
        
        var completedTreatment = new Treatment(
            patientId,
            TreatmentType.Cleaning,
            "Completed Treatment",
            "Description",
            100.00m);
        completedTreatment.Start();
        completedTreatment.Complete(100.00m, "Treatment completed");

        var treatments = new List<Treatment> { completedTreatment };

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
        
        _unitOfWorkMock.Setup(x => x.Treatments.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatments);

        _unitOfWorkMock.Setup(x => x.Patients.UpdateAsync(patient, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Treatments.GetByPatientIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.UpdateAsync(patient, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}