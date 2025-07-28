using AutoMapper;
using DentalTrack.Application.Commands.Patients;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Handlers.Patients;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Patients;

public class UpdatePatientHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdatePatientHandler _handler;

    public UpdatePatientHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdatePatientHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_UpdatesAndReturnsPatient()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        var updatePatientDto = new UpdatePatientDto
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@email.com",
            Phone = "987-654-3210",
            DateOfBirth = DateTime.Now.AddYears(-25),
            Address = "123 Main St",
            EmergencyContact = "John Smith",
            EmergencyPhone = "555-1234",
            MedicalHistory = "No significant history",
            Allergies = "None"
        };

        var command = new UpdatePatientCommand(patientId, updatePatientDto);

        var existingPatient = new Patient("John", "Doe", "john.doe@email.com", DateTime.Now.AddYears(-30), "123-456-7890");

        var updatedPatientDto = new PatientDto
        {
            Id = patientId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@email.com",
            Phone = "987-654-3210"
        };

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPatient);

        _unitOfWorkMock.Setup(x => x.Patients.EmailExistsAsync(updatePatientDto.Email, patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Patients.UpdateAsync(existingPatient, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mapperMock.Setup(x => x.Map<PatientDto>(existingPatient))
            .Returns(updatedPatientDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedPatientDto.Id, result.Id);
        Assert.Equal(updatedPatientDto.FirstName, result.FirstName);
        Assert.Equal(updatedPatientDto.LastName, result.LastName);
        Assert.Equal(updatedPatientDto.Email, result.Email);
        Assert.Equal(updatedPatientDto.Phone, result.Phone);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.EmailExistsAsync(updatePatientDto.Email, patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.UpdateAsync(existingPatient, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<PatientDto>(existingPatient), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentPatient_ThrowsInvalidOperationException()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        var updatePatientDto = new UpdatePatientDto
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@email.com"
        };

        var command = new UpdatePatientCommand(patientId, updatePatientDto);

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains($"Patient with ID {patientId} not found", exception.Message);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.EmailExistsAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.Patients.UpdateAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        var updatePatientDto = new UpdatePatientDto
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "existing@email.com"
        };

        var command = new UpdatePatientCommand(patientId, updatePatientDto);

        var existingPatient = new Patient("John", "Doe", "john.doe@email.com", DateTime.Now.AddYears(-30), "123-456-7890");

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPatient);

        _unitOfWorkMock.Setup(x => x.Patients.EmailExistsAsync(updatePatientDto.Email, patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains($"Patient with email {updatePatientDto.Email} already exists", exception.Message);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.EmailExistsAsync(updatePatientDto.Email, patientId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Patients.UpdateAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

}