using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Infrastructure.Repositories;
using FluentAssertions;

namespace DentalTrack.Infrastructure.Tests.Repositories;

public class TreatmentRepositoryTests : TestBase
{
    private readonly TreatmentRepository _repository;
    private readonly Patient _testPatient;

    public TreatmentRepositoryTests()
    {
        _repository = new TreatmentRepository(Context);
        _testPatient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        Context.Patients.Add(_testPatient);
        Context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_WithNoTreatments_ShouldReturnEmptyList()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WithTreatments_ShouldReturnAllTreatments()
    {
        // Arrange
        var treatment1 = new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Routine Cleaning", "Regular cleaning", 150.00m, DateTime.UtcNow);
        var treatment2 = new Treatment(_testPatient.Id, TreatmentType.Filling, "Cavity Filling", "Fill cavity", 300.00m, DateTime.UtcNow.AddDays(1));

        await Context.Treatments.AddRangeAsync(treatment1, treatment2);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Title == "Routine Cleaning");
        result.Should().Contain(t => t.Title == "Cavity Filling");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingTreatment_ShouldReturnTreatment()
    {
        // Arrange
        var treatment = new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Routine Cleaning", "Regular cleaning", 150.00m, DateTime.UtcNow);
        await Context.Treatments.AddAsync(treatment);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(treatment.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Routine Cleaning");
        result.Type.Should().Be(TreatmentType.Cleaning);
        result.EstimatedCost.Should().Be(150.00m);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentTreatment_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByPatientIdAsync_WithExistingTreatments_ShouldReturnPatientTreatments()
    {
        // Arrange
        var patient2 = new Patient("Jane", "Smith", "jane@example.com", new DateTime(1990, 3, 20));
        await Context.Patients.AddAsync(patient2);
        await Context.SaveChangesAsync();

        var treatment1 = new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Patient 1 Cleaning", "Regular cleaning", 150.00m, DateTime.UtcNow);
        var treatment2 = new Treatment(_testPatient.Id, TreatmentType.Filling, "Patient 1 Filling", "Fill cavity", 300.00m, DateTime.UtcNow.AddDays(1));
        var treatment3 = new Treatment(patient2.Id, TreatmentType.Cleaning, "Patient 2 Cleaning", "Regular cleaning", 150.00m, DateTime.UtcNow);

        await Context.Treatments.AddRangeAsync(treatment1, treatment2, treatment3);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByPatientIdAsync(_testPatient.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(t => t.PatientId.Should().Be(_testPatient.Id));
        result.Should().Contain(t => t.Title == "Patient 1 Cleaning");
        result.Should().Contain(t => t.Title == "Patient 1 Filling");
    }

    [Fact]
    public async Task GetByPatientIdAsync_WithNoTreatments_ShouldReturnEmptyList()
    {
        // Act
        var result = await _repository.GetByPatientIdAsync(_testPatient.Id);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPagedAsync_WithNoFilters_ShouldReturnPagedResults()
    {
        // Arrange
        var treatments = new[]
        {
            new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Treatment 1", "Description 1", 150.00m, DateTime.UtcNow),
            new Treatment(_testPatient.Id, TreatmentType.Filling, "Treatment 2", "Description 2", 300.00m, DateTime.UtcNow.AddDays(1)),
            new Treatment(_testPatient.Id, TreatmentType.RootCanal, "Treatment 3", "Description 3", 800.00m, DateTime.UtcNow.AddDays(2))
        };

        await Context.Treatments.AddRangeAsync(treatments);
        await Context.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(1, 2);

        // Assert
        items.Should().HaveCount(2);
        totalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetPagedAsync_WithPatientIdFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var patient2 = new Patient("Jane", "Smith", "jane@example.com", new DateTime(1990, 3, 20));
        await Context.Patients.AddAsync(patient2);
        await Context.SaveChangesAsync();

        var treatments = new[]
        {
            new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Patient 1 Treatment", "Description 1", 150.00m, DateTime.UtcNow),
            new Treatment(patient2.Id, TreatmentType.Filling, "Patient 2 Treatment", "Description 2", 300.00m, DateTime.UtcNow.AddDays(1))
        };

        await Context.Treatments.AddRangeAsync(treatments);
        await Context.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(1, 10, _testPatient.Id);

        // Assert
        items.Should().HaveCount(1);
        totalCount.Should().Be(1);
        items.First().PatientId.Should().Be(_testPatient.Id);
    }

    [Fact]
    public async Task GetPagedAsync_WithStatusFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var treatment1 = new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Treatment 1", "Description 1", 150.00m, DateTime.UtcNow);
        var treatment2 = new Treatment(_testPatient.Id, TreatmentType.Filling, "Treatment 2", "Description 2", 300.00m, DateTime.UtcNow.AddDays(1));
        
        // Start treatment2 to change its status
        treatment2.Start();

        await Context.Treatments.AddRangeAsync(treatment1, treatment2);
        await Context.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(1, 10, status: "InProgress");

        // Assert
        items.Should().HaveCount(1);
        totalCount.Should().Be(1);
        items.First().Status.Should().Be(TreatmentStatus.InProgress);
    }

    [Fact]
    public async Task GetPagedAsync_WithSorting_ShouldReturnSortedResults()
    {
        // Arrange
        var treatments = new[]
        {
            new Treatment(_testPatient.Id, TreatmentType.RootCanal, "Charlie Treatment", "Description", 800.00m, DateTime.UtcNow.AddDays(2)),
            new Treatment(_testPatient.Id, TreatmentType.Filling, "Alice Treatment", "Description", 300.00m, DateTime.UtcNow.AddDays(1)),
            new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Bob Treatment", "Description", 150.00m, DateTime.UtcNow)
        };

        await Context.Treatments.AddRangeAsync(treatments);
        await Context.SaveChangesAsync();

        // Act - Sort by Type ascending 
        var (items, _) = await _repository.GetPagedAsync(1, 10, sortBy: "Type", sortDescending: false);

        // Assert
        var sortedItems = items.ToList();
        sortedItems[0].Type.Should().Be(TreatmentType.Cleaning);
        sortedItems[1].Type.Should().Be(TreatmentType.Filling);
        sortedItems[2].Type.Should().Be(TreatmentType.RootCanal);
    }

    [Fact]
    public async Task AddAsync_WithValidTreatment_ShouldAddTreatment()
    {
        // Arrange
        var treatment = new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Routine Cleaning", "Regular cleaning", 150.00m, DateTime.UtcNow);

        // Act
        await _repository.AddAsync(treatment);
        await Context.SaveChangesAsync();

        // Assert
        var addedTreatment = await Context.Treatments.FindAsync(treatment.Id);
        addedTreatment.Should().NotBeNull();
        addedTreatment!.Title.Should().Be("Routine Cleaning");
    }

    [Fact]
    public async Task Update_WithValidTreatment_ShouldUpdateTreatment()
    {
        // Arrange
        var treatment = new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Original Title", "Original description", 150.00m, DateTime.UtcNow);
        await Context.Treatments.AddAsync(treatment);
        await Context.SaveChangesAsync();

        // Act
        treatment.UpdateDetails("Updated Title", "Updated description", 200.00m);
        await _repository.UpdateAsync(treatment);
        await Context.SaveChangesAsync();

        // Assert
        var updatedTreatment = await Context.Treatments.FindAsync(treatment.Id);
        updatedTreatment.Should().NotBeNull();
        updatedTreatment!.Title.Should().Be("Updated Title");
        updatedTreatment.Description.Should().Be("Updated description");
        updatedTreatment.EstimatedCost.Should().Be(200.00m);
    }

    [Fact]
    public async Task Delete_WithValidTreatment_ShouldRemoveTreatment()
    {
        // Arrange
        var treatment = new Treatment(_testPatient.Id, TreatmentType.Cleaning, "Routine Cleaning", "Regular cleaning", 150.00m, DateTime.UtcNow);
        await Context.Treatments.AddAsync(treatment);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(treatment);
        await Context.SaveChangesAsync();

        // Assert
        var deletedTreatment = await Context.Treatments.FindAsync(treatment.Id);
        deletedTreatment.Should().BeNull();
    }
}