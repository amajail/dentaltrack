using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Handlers.Patients;
using DentalTrack.Application.Queries.Patients;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Patients;

public class GetPatientByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPatientByIdHandler _handler;

    public GetPatientByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPatientByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingPatient_ReturnsPatientDto()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var query = new GetPatientByIdQuery(patientId);

        var patient = new Patient("John", "Doe", "john.doe@email.com", DateTime.Now.AddYears(-30), "123-456-7890");
        
        var patientDto = new PatientDto
        {
            Id = patientId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@email.com",
            Phone = "123-456-7890"
        };

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
        
        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(patientDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patientDto.Id, result.Id);
        Assert.Equal(patientDto.FirstName, result.FirstName);
        Assert.Equal(patientDto.LastName, result.LastName);
        Assert.Equal(patientDto.Email, result.Email);
        Assert.Equal(patientDto.Phone, result.Phone);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<PatientDto>(patient), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentPatient_ReturnsNull()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var query = new GetPatientByIdQuery(patientId);

        _unitOfWorkMock.Setup(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);

        _unitOfWorkMock.Verify(x => x.Patients.GetByIdAsync(patientId, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<PatientDto>(It.IsAny<Patient>()), Times.Never);
    }

}