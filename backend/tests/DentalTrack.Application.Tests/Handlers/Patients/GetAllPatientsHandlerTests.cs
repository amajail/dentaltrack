using AutoMapper;
using DentalTrack.Application.Common;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Handlers.Patients;
using DentalTrack.Application.Mappings;
using DentalTrack.Application.Queries.Patients;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace DentalTrack.Application.Tests.Handlers.Patients;

public class GetAllPatientsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly IMapper _mapper;
    private readonly GetAllPatientsHandler _handler;

    public GetAllPatientsHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPatientRepository = new Mock<IPatientRepository>();
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        
        _mockUnitOfWork.Setup(x => x.Patients).Returns(_mockPatientRepository.Object);
        
        _handler = new GetAllPatientsHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WithExistingPatients_ShouldReturnPagedPatientDtos()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new("John", "Doe", "john@example.com", new DateTime(1985, 5, 15)),
            new("Jane", "Smith", "jane@example.com", new DateTime(1990, 3, 20)),
            new("Bob", "Johnson", "bob@example.com", new DateTime(1975, 12, 10))
        };

        var query = new GetAllPatientsQuery { Page = 1, PageSize = 10 };

        _mockPatientRepository
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((patients, 3));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        var resultList = result.Items.ToList();
        resultList[0].FirstName.Should().Be("John");
        resultList[0].LastName.Should().Be("Doe");
        resultList[0].Email.Should().Be("john@example.com");
        resultList[0].FullName.Should().Be("John Doe");

        resultList[1].FirstName.Should().Be("Jane");
        resultList[1].LastName.Should().Be("Smith");
        resultList[1].Email.Should().Be("jane@example.com");
        resultList[1].FullName.Should().Be("Jane Smith");

        resultList[2].FirstName.Should().Be("Bob");
        resultList[2].LastName.Should().Be("Johnson");
        resultList[2].Email.Should().Be("bob@example.com");
        resultList[2].FullName.Should().Be("Bob Johnson");

        _mockPatientRepository.Verify(
            x => x.GetPagedAsync(
                query.Page, 
                query.PageSize, 
                query.Search, 
                query.SortBy, 
                query.SortDescending, 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoPatients_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var patients = new List<Patient>();
        var query = new GetAllPatientsQuery { Page = 1, PageSize = 10 };

        _mockPatientRepository
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((patients, 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        _mockPatientRepository.Verify(
            x => x.GetPagedAsync(
                query.Page, 
                query.PageSize, 
                query.Search, 
                query.SortBy, 
                query.SortDescending, 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapPatientsCorrectly()
    {
        // Arrange
        var patient = new Patient(
            "Test", 
            "User", 
            "test@example.com", 
            new DateTime(1980, 6, 15),
            "555-1234",
            "123 Test St",
            "Emergency Contact",
            "555-9999",
            "Medical history",
            "Allergies");

        var patients = new List<Patient> { patient };
        var query = new GetAllPatientsQuery { Page = 1, PageSize = 10 };

        _mockPatientRepository
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((patients, 1));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var patientDto = result.Items.First();
        patientDto.Should().NotBeNull();
        patientDto.Id.Should().Be(patient.Id);
        patientDto.FirstName.Should().Be(patient.FirstName);
        patientDto.LastName.Should().Be(patient.LastName);
        patientDto.Email.Should().Be(patient.Email);
        patientDto.Phone.Should().Be(patient.Phone);
        patientDto.Address.Should().Be(patient.Address);
        patientDto.EmergencyContact.Should().Be(patient.EmergencyContact);
        patientDto.EmergencyPhone.Should().Be(patient.EmergencyPhone);
        patientDto.MedicalHistory.Should().Be(patient.MedicalHistory);
        patientDto.Allergies.Should().Be(patient.Allergies);
        patientDto.IsActive.Should().Be(patient.IsActive);
        patientDto.FullName.Should().Be(patient.GetFullName());
        patientDto.Age.Should().Be(patient.GetAge());
        patientDto.CreatedAt.Should().Be(patient.CreatedAt);
        patientDto.UpdatedAt.Should().Be(patient.UpdatedAt);
    }

    [Fact]
    public async Task Handle_WithPaginationParameters_ShouldPassCorrectParameters()
    {
        // Arrange
        var patients = new List<Patient>();
        var query = new GetAllPatientsQuery 
        { 
            Page = 2, 
            PageSize = 5,
            Search = "John",
            SortBy = "FirstName",
            SortDescending = true
        };

        _mockPatientRepository
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((patients, 0));

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockPatientRepository.Verify(
            x => x.GetPagedAsync(
                2,          // page
                5,          // pageSize
                "John",     // search
                "FirstName", // sortBy
                true,       // sortDescending
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}