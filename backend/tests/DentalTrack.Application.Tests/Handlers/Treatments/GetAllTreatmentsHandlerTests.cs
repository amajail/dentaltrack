using AutoMapper;
using DentalTrack.Application.Common;
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

public class GetAllTreatmentsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITreatmentRepository> _mockTreatmentRepository;
    private readonly IMapper _mapper;
    private readonly GetAllTreatmentsHandler _handler;

    public GetAllTreatmentsHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTreatmentRepository = new Mock<ITreatmentRepository>();
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        
        _mockUnitOfWork.Setup(x => x.Treatments).Returns(_mockTreatmentRepository.Object);
        
        _handler = new GetAllTreatmentsHandler(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task Handle_WithExistingTreatments_ShouldReturnPagedTreatmentDtos()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        var treatments = new List<Treatment>
        {
            new(patient.Id, TreatmentType.Cleaning, "Routine Cleaning", "Regular dental cleaning", 150.00m, DateTime.UtcNow),
            new(patient.Id, TreatmentType.Filling, "Cavity Filling", "Fill cavity on tooth #12", 300.00m, DateTime.UtcNow.AddDays(1))
        };

        // Set the Patient property for mapping
        treatments[0].GetType().GetProperty("Patient")?.SetValue(treatments[0], patient);
        treatments[1].GetType().GetProperty("Patient")?.SetValue(treatments[1], patient);

        var query = new GetAllTreatmentsQuery { Page = 1, PageSize = 10 };

        _mockTreatmentRepository
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((treatments, 2));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        var resultList = result.Items.ToList();
        resultList[0].Type.Should().Be(TreatmentType.Cleaning);
        resultList[0].Title.Should().Be("Routine Cleaning");
        resultList[1].Type.Should().Be(TreatmentType.Filling);
        resultList[1].Title.Should().Be("Cavity Filling");
    }

    [Fact]
    public async Task Handle_WithNoTreatments_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var treatments = new List<Treatment>();
        var query = new GetAllTreatmentsQuery { Page = 1, PageSize = 10 };

        _mockTreatmentRepository
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((treatments, 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_WithPaginationAndFilters_ShouldPassCorrectParameters()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var treatments = new List<Treatment>();
        var query = new GetAllTreatmentsQuery 
        { 
            Page = 2, 
            PageSize = 5,
            PatientId = patientId,
            Status = "InProgress",
            SortBy = "StartDate",
            SortDescending = false
        };

        _mockTreatmentRepository
            .Setup(x => x.GetPagedAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((treatments, 0));

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTreatmentRepository.Verify(
            x => x.GetPagedAsync(
                2,              // page
                5,              // pageSize
                patientId,      // patientId
                "InProgress",   // status
                "StartDate",    // sortBy
                false,          // sortDescending
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}