using DentalTrack.Domain.Entities;
using DentalTrack.Infrastructure.Repositories;
using FluentAssertions;

namespace DentalTrack.Infrastructure.Tests.Repositories;

public class PatientRepositoryTests : TestBase
{
    private readonly PatientRepository _repository;

    public PatientRepositoryTests()
    {
        _repository = new PatientRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_WithNoPatients_ShouldReturnEmptyList()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WithPatients_ShouldReturnAllPatients()
    {
        // Arrange
        var patient1 = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        var patient2 = new Patient("Jane", "Smith", "jane@example.com", new DateTime(1990, 3, 20));

        await Context.Patients.AddRangeAsync(patient1, patient2);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Email == "john@example.com");
        result.Should().Contain(p => p.Email == "jane@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingPatient_ShouldReturnPatient()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        await Context.Patients.AddAsync(patient);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(patient.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("john@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentPatient_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPagedAsync_WithNoFilters_ShouldReturnPagedResults()
    {
        // Arrange
        var patients = new[]
        {
            new Patient("Alice", "Johnson", "alice@example.com", new DateTime(1985, 1, 1)),
            new Patient("Bob", "Williams", "bob@example.com", new DateTime(1990, 2, 2)),
            new Patient("Charlie", "Brown", "charlie@example.com", new DateTime(1995, 3, 3))
        };

        await Context.Patients.AddRangeAsync(patients);
        await Context.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(1, 2);

        // Assert
        items.Should().HaveCount(2);
        totalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetPagedAsync_WithSearchFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var patients = new[]
        {
            new Patient("John", "Doe", "john@example.com", new DateTime(1985, 1, 1)),
            new Patient("Jane", "Doe", "jane@example.com", new DateTime(1990, 2, 2)),
            new Patient("Bob", "Smith", "bob@example.com", new DateTime(1995, 3, 3))
        };

        await Context.Patients.AddRangeAsync(patients);
        await Context.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(1, 10, "Doe");

        // Assert
        items.Should().HaveCount(2);
        totalCount.Should().Be(2);
        items.Should().AllSatisfy(p => p.LastName.Should().Be("Doe"));
    }

    [Fact]
    public async Task GetPagedAsync_WithSorting_ShouldReturnSortedResults()
    {
        // Arrange
        var patients = new[]
        {
            new Patient("Charlie", "Brown", "charlie@example.com", new DateTime(1985, 1, 1)),
            new Patient("Alice", "Johnson", "alice@example.com", new DateTime(1990, 2, 2)),
            new Patient("Bob", "Williams", "bob@example.com", new DateTime(1995, 3, 3))
        };

        await Context.Patients.AddRangeAsync(patients);
        await Context.SaveChangesAsync();

        // Act - Sort by FirstName ascending
        var (items, _) = await _repository.GetPagedAsync(1, 10, sortBy: "FirstName", sortDescending: false);

        // Assert
        var sortedItems = items.ToList();
        sortedItems[0].FirstName.Should().Be("Alice");
        sortedItems[1].FirstName.Should().Be("Bob");
        sortedItems[2].FirstName.Should().Be("Charlie");
    }

    [Fact]
    public async Task GetPagedAsync_WithDescendingSorting_ShouldReturnSortedResults()
    {
        // Arrange
        var patients = new[]
        {
            new Patient("Alice", "Johnson", "alice@example.com", new DateTime(1985, 1, 1)),
            new Patient("Bob", "Williams", "bob@example.com", new DateTime(1990, 2, 2)),
            new Patient("Charlie", "Brown", "charlie@example.com", new DateTime(1995, 3, 3))
        };

        await Context.Patients.AddRangeAsync(patients);
        await Context.SaveChangesAsync();

        // Act - Sort by LastName descending
        var (items, _) = await _repository.GetPagedAsync(1, 10, sortBy: "LastName", sortDescending: true);

        // Assert
        var sortedItems = items.ToList();
        sortedItems[0].LastName.Should().Be("Williams");
        sortedItems[1].LastName.Should().Be("Johnson");
        sortedItems[2].LastName.Should().Be("Brown");
    }

    [Fact]
    public async Task AddAsync_WithValidPatient_ShouldAddPatient()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));

        // Act
        await _repository.AddAsync(patient);
        await Context.SaveChangesAsync();

        // Assert
        var addedPatient = await Context.Patients.FindAsync(patient.Id);
        addedPatient.Should().NotBeNull();
        addedPatient!.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task Update_WithValidPatient_ShouldUpdatePatient()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        await Context.Patients.AddAsync(patient);
        await Context.SaveChangesAsync();

        // Act
        patient.UpdatePersonalInfo("Jane", "Smith", "jane@example.com", patient.DateOfBirth);
        await _repository.UpdateAsync(patient);
        await Context.SaveChangesAsync();

        // Assert
        var updatedPatient = await Context.Patients.FindAsync(patient.Id);
        updatedPatient.Should().NotBeNull();
        updatedPatient!.FirstName.Should().Be("Jane");
        updatedPatient.LastName.Should().Be("Smith");
        updatedPatient.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public async Task Delete_WithValidPatient_ShouldRemovePatient()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        await Context.Patients.AddAsync(patient);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(patient);
        await Context.SaveChangesAsync();

        // Assert
        var deletedPatient = await Context.Patients.FindAsync(patient.Id);
        deletedPatient.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnPatient()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        await Context.Patients.AddAsync(patient);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("john@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task GetByEmailAsync_WithNonExistentEmail_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        result.Should().BeNull();
    }
}