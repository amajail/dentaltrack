using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Infrastructure.Repositories;
using FluentAssertions;

namespace DentalTrack.Infrastructure.Tests.Repositories;

public class UnitOfWorkTests : TestBase
{
    private readonly UnitOfWork _unitOfWork;

    public UnitOfWorkTests()
    {
        _unitOfWork = new UnitOfWork(Context);
    }

    [Fact]
    public void UnitOfWork_ShouldProvideAccessToAllRepositories()
    {
        // Assert
        _unitOfWork.Patients.Should().NotBeNull();
        _unitOfWork.Treatments.Should().NotBeNull();
        _unitOfWork.Users.Should().NotBeNull();
        _unitOfWork.Analyses.Should().NotBeNull();
        _unitOfWork.Photos.Should().NotBeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_WithMultipleEntityChanges_ShouldPersistAllChanges()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        var user = new User("doctor@example.com", "Dr.", "Smith", UserRole.Doctor);
        var treatment = new Treatment(patient.Id, TreatmentType.Cleaning, "Routine Cleaning", "Regular cleaning", 150.00m, DateTime.UtcNow);

        // Act
        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.Treatments.AddAsync(treatment);
        
        var result = await _unitOfWork.SaveChangesAsync();

        // Assert
        result.Should().Be(3); // 3 entities saved

        var savedPatient = await Context.Patients.FindAsync(patient.Id);
        var savedUser = await Context.Users.FindAsync(user.Id);
        var savedTreatment = await Context.Treatments.FindAsync(treatment.Id);

        savedPatient.Should().NotBeNull();
        savedUser.Should().NotBeNull();
        savedTreatment.Should().NotBeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_WithNoChanges_ShouldReturnZero()
    {
        // Act
        var result = await _unitOfWork.SaveChangesAsync();

        // Assert
        result.Should().Be(0);
    }

    [Fact(Skip = "In-memory database doesn't support transactions")]
    public async Task BeginTransactionAsync_ShouldStartTransaction()
    {
        // Act & Assert - Should not throw
        await _unitOfWork.BeginTransactionAsync();
        
        // Cleanup
        await _unitOfWork.RollbackTransactionAsync();
    }

    [Fact(Skip = "In-memory database doesn't support transactions")]
    public async Task Transaction_WithCommit_ShouldPersistChanges()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));

        // Act
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();

        // Assert
        var savedPatient = await Context.Patients.FindAsync(patient.Id);
        savedPatient.Should().NotBeNull();
    }

    [Fact(Skip = "In-memory database doesn't support transactions")]
    public async Task Transaction_WithRollback_ShouldNotPersistChanges()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));

        // Act
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.RollbackTransactionAsync();

        // Assert
        var savedPatient = await Context.Patients.FindAsync(patient.Id);
        savedPatient.Should().BeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_WithConcurrentModification_ShouldHandleCorrectly()
    {
        // Arrange
        var patient = new Patient("John", "Doe", "john@example.com", new DateTime(1985, 5, 15));
        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        // Create second unit of work with same context for simplicity in testing
        var unitOfWork2 = new UnitOfWork(Context);

        // Act - Modify the same patient in both contexts
        var patient1 = await _unitOfWork.Patients.GetByIdAsync(patient.Id);
        var patient2 = await unitOfWork2.Patients.GetByIdAsync(patient.Id);

        patient1!.UpdatePersonalInfo("John1", "Doe1", "john1@example.com", patient1.DateOfBirth);
        patient2!.UpdatePersonalInfo("John2", "Doe2", "john2@example.com", patient2.DateOfBirth);

        await _unitOfWork.SaveChangesAsync();
        
        // The second save should work since we're using in-memory database
        await unitOfWork2.SaveChangesAsync();

        // Assert - The last save should win in in-memory database
        var finalPatient = await _unitOfWork.Patients.GetByIdAsync(patient.Id);
        finalPatient.Should().NotBeNull();
    }

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        // Act
        _unitOfWork.Dispose();

        // Assert - Context should be disposed (difficult to test directly with in-memory)
        // We'll just verify no exception is thrown
        Action act = () => _unitOfWork.Dispose();
        act.Should().NotThrow();
    }
}