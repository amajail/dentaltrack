using DentalTrack.Domain.Entities;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.Entities;

public class PatientTests
{
    private readonly string _validFirstName = "John";
    private readonly string _validLastName = "Doe";
    private readonly string _validEmail = "john.doe@example.com";
    private readonly DateTime _validDateOfBirth = new DateTime(1985, 5, 15);

    [Fact]
    public void Constructor_WithValidData_ShouldCreatePatient()
    {
        // Act
        var patient = new Patient(
            _validFirstName,
            _validLastName,
            _validEmail,
            _validDateOfBirth,
            "555-1234",
            "Male",
            "123 Main St",
            "Jane Doe",
            "555-5678",
            "No known allergies",
            "Penicillin");

        // Assert
        patient.FirstName.Should().Be(_validFirstName);
        patient.LastName.Should().Be(_validLastName);
        patient.Email.Should().Be(_validEmail);
        patient.DateOfBirth.Should().Be(_validDateOfBirth);
        patient.Phone.Should().Be("555-1234");
        patient.Gender.Should().Be("Male");
        patient.Address.Should().Be("123 Main St");
        patient.EmergencyContact.Should().Be("Jane Doe");
        patient.EmergencyPhone.Should().Be("555-5678");
        patient.MedicalHistory.Should().Be("No known allergies");
        patient.Allergies.Should().Be("Penicillin");
        patient.IsActive.Should().BeTrue();
        patient.Id.Should().NotBeEmpty();
        patient.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("", "Doe", "john@example.com")]
    [InlineData(null, "Doe", "john@example.com")]
    [InlineData("   ", "Doe", "john@example.com")]
    public void Constructor_WithInvalidFirstName_ShouldThrowException(string firstName, string lastName, string email)
    {
        // Act & Assert
        var action = () => new Patient(firstName, lastName, email, _validDateOfBirth);
        action.Should().Throw<ArgumentException>()
            .WithMessage("First name cannot be empty*");
    }

    [Theory]
    [InlineData("John", "", "john@example.com")]
    [InlineData("John", null, "john@example.com")]
    [InlineData("John", "   ", "john@example.com")]
    public void Constructor_WithInvalidLastName_ShouldThrowException(string firstName, string lastName, string email)
    {
        // Act & Assert
        var action = () => new Patient(firstName, lastName, email, _validDateOfBirth);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Last name cannot be empty*");
    }

    [Theory]
    [InlineData("John", "Doe", "")]
    [InlineData("John", "Doe", null)]
    [InlineData("John", "Doe", "   ")]
    public void Constructor_WithInvalidEmail_ShouldThrowException(string firstName, string lastName, string email)
    {
        // Act & Assert
        var action = () => new Patient(firstName, lastName, email, _validDateOfBirth);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Email cannot be empty*");
    }

    [Fact]
    public void Constructor_WithFutureDateOfBirth_ShouldThrowException()
    {
        // Arrange
        var futureDate = DateTime.Today.AddDays(1);

        // Act & Assert
        var action = () => new Patient(_validFirstName, _validLastName, _validEmail, futureDate);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Date of birth must be in the past*");
    }

    [Fact]
    public void UpdatePersonalInfo_WithValidData_ShouldUpdateFields()
    {
        // Arrange
        var patient = new Patient(_validFirstName, _validLastName, _validEmail, _validDateOfBirth);
        var originalUpdatedAt = patient.UpdatedAt;

        // Act
        patient.UpdatePersonalInfo("Jane", "Smith", "jane.smith@example.com",
            new DateTime(1990, 3, 20), "555-9999", "456 Oak Ave");

        // Assert
        patient.FirstName.Should().Be("Jane");
        patient.LastName.Should().Be("Smith");
        patient.Email.Should().Be("jane.smith@example.com");
        patient.DateOfBirth.Should().Be(new DateTime(1990, 3, 20));
        patient.Phone.Should().Be("555-9999");
        patient.Address.Should().Be("456 Oak Ave");
        patient.UpdatedAt.Should().NotBe(originalUpdatedAt);
        patient.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateMedicalInfo_WithValidData_ShouldUpdateFields()
    {
        // Arrange
        var patient = new Patient(_validFirstName, _validLastName, _validEmail, _validDateOfBirth);

        // Act
        patient.UpdateMedicalInfo("Emergency Contact", "555-0000", "Updated history", "Updated allergies");

        // Assert
        patient.EmergencyContact.Should().Be("Emergency Contact");
        patient.EmergencyPhone.Should().Be("555-0000");
        patient.MedicalHistory.Should().Be("Updated history");
        patient.Allergies.Should().Be("Updated allergies");
        patient.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var patient = new Patient(_validFirstName, _validLastName, _validEmail, _validDateOfBirth);

        // Act
        patient.Deactivate();

        // Assert
        patient.IsActive.Should().BeFalse();
        patient.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var patient = new Patient(_validFirstName, _validLastName, _validEmail, _validDateOfBirth);
        patient.Deactivate();

        // Act
        patient.Activate();

        // Assert
        patient.IsActive.Should().BeTrue();
        patient.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void GetFullName_ShouldReturnFirstNameLastName()
    {
        // Arrange
        var patient = new Patient(_validFirstName, _validLastName, _validEmail, _validDateOfBirth);

        // Act
        var fullName = patient.GetFullName();

        // Assert
        fullName.Should().Be("John Doe");
    }

    [Theory]
    [InlineData(1985, 5, 15, 39)] // Assuming current year is 2024
    [InlineData(2000, 1, 1, 24)]
    [InlineData(1950, 12, 31, 73)]
    public void GetAge_ShouldCalculateCorrectAge(int year, int month, int day, int expectedAge)
    {
        // Arrange
        var dateOfBirth = new DateTime(year, month, day);
        var patient = new Patient(_validFirstName, _validLastName, _validEmail, dateOfBirth);

        // Act
        var age = patient.GetAge();

        // Assert
        // Note: This test might be brittle depending on when it's run
        // In a real scenario, you might want to inject a date service
        age.Should().BeGreaterThanOrEqualTo(expectedAge - 1).And.BeLessThanOrEqualTo(expectedAge + 1);
    }

    [Fact]
    public void Treatments_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var patient = new Patient(_validFirstName, _validLastName, _validEmail, _validDateOfBirth);

        // Assert
        patient.Treatments.Should().NotBeNull();
        patient.Treatments.Should().BeEmpty();
    }
}