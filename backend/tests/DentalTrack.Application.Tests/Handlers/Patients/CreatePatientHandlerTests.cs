using AutoMapper;
using DentalTrack.Application.Commands.Patients;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Handlers.Patients;
using DentalTrack.Application.Mappings;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Patients;

public class CreatePatientHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly IMapper _mapper;
    private readonly CreatePatientHandler _handler;

    public CreatePatientHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPatientRepository = new Mock<IPatientRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _mockUnitOfWork.Setup(x => x.Patients).Returns(_mockPatientRepository.Object);

        _handler = new CreatePatientHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldCreatePatientAndReturnDto()
    {
        // Arrange
        var createPatientDto = new CreatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = new DateTime(1985, 5, 15),
            Phone = "555-1234",
            Address = "123 Main St"
        };

        var command = new CreatePatientCommand(createPatientDto);

        _mockPatientRepository
            .Setup(x => x.EmailExistsAsync(createPatientDto.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockPatientRepository
            .Setup(x => x.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient p, CancellationToken ct) => p);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(createPatientDto.FirstName);
        result.LastName.Should().Be(createPatientDto.LastName);
        result.Email.Should().Be(createPatientDto.Email);
        result.DateOfBirth.Should().Be(createPatientDto.DateOfBirth);
        result.Phone.Should().Be(createPatientDto.Phone);
        result.Address.Should().Be(createPatientDto.Address);
        result.IsActive.Should().BeTrue();
        result.Id.Should().NotBeEmpty();

        _mockPatientRepository.Verify(
            x => x.EmailExistsAsync(createPatientDto.Email, null, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockPatientRepository.Verify(
            x => x.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var createPatientDto = new CreatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "existing@example.com",
            DateOfBirth = new DateTime(1985, 5, 15)
        };

        var command = new CreatePatientCommand(createPatientDto);

        _mockPatientRepository
            .Setup(x => x.EmailExistsAsync(createPatientDto.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Patient with email {createPatientDto.Email} already exists");

        _mockPatientRepository.Verify(
            x => x.EmailExistsAsync(createPatientDto.Email, null, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockPatientRepository.Verify(
            x => x.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithMinimalData_ShouldCreatePatientSuccessfully()
    {
        // Arrange
        var createPatientDto = new CreatePatientDto
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            DateOfBirth = new DateTime(1990, 3, 20)
        };

        var command = new CreatePatientCommand(createPatientDto);

        _mockPatientRepository
            .Setup(x => x.EmailExistsAsync(createPatientDto.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockPatientRepository
            .Setup(x => x.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient p, CancellationToken ct) => p);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(createPatientDto.FirstName);
        result.LastName.Should().Be(createPatientDto.LastName);
        result.Email.Should().Be(createPatientDto.Email);
        result.DateOfBirth.Should().Be(createPatientDto.DateOfBirth);
        result.Phone.Should().BeNull();
        result.Address.Should().BeNull();
        result.FullName.Should().Be("Jane Smith");
    }

    [Fact]
    public async Task Handle_ShouldMapPatientCorrectly()
    {
        // Arrange
        var createPatientDto = new CreatePatientDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test.user@example.com",
            DateOfBirth = new DateTime(1980, 1, 1),
            Phone = "555-0000",
            Address = "Test Address",
            EmergencyContact = "Emergency Contact",
            EmergencyPhone = "555-9999",
            MedicalHistory = "Test history",
            Allergies = "Test allergies"
        };

        var command = new CreatePatientCommand(createPatientDto);

        _mockPatientRepository
            .Setup(x => x.EmailExistsAsync(createPatientDto.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockPatientRepository
            .Setup(x => x.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient p, CancellationToken ct) => p);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.EmergencyContact.Should().Be(createPatientDto.EmergencyContact);
        result.EmergencyPhone.Should().Be(createPatientDto.EmergencyPhone);
        result.MedicalHistory.Should().Be(createPatientDto.MedicalHistory);
        result.Allergies.Should().Be(createPatientDto.Allergies);
        result.Age.Should().BeGreaterThan(0);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}