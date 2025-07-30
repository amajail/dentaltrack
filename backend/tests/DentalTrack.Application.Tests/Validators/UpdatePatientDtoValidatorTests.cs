using DentalTrack.Application.DTOs;
using DentalTrack.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace DentalTrack.Application.Tests.Validators;

public class UpdatePatientDtoValidatorTests
{
    private readonly UpdatePatientDtoValidator _validator;

    public UpdatePatientDtoValidatorTests()
    {
        _validator = new UpdatePatientDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "5551234567",
            DateOfBirth = DateTime.Today.AddYears(-30),
            Gender = "Male",
            Address = "123 Main St",
            EmergencyContact = "Jane Doe",
            EmergencyPhone = "5555678901",
            MedicalHistory = "No significant medical history",
            Allergies = "None"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyFirstName_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name is required");
    }

    [Fact]
    public void Validate_WithTooLongFirstName_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = new string('A', 101), // 101 characters
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name cannot exceed 100 characters");
    }

    [Fact]
    public void Validate_WithEmptyLastName_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name is required");
    }

    [Fact]
    public void Validate_WithTooLongLastName_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = new string('D', 101), // 101 characters
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name cannot exceed 100 characters");
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email must be a valid email address");
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "",
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Fact]
    public void Validate_WithTooLongEmail_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = new string('a', 250) + "@example.com", // Over 255 characters
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email cannot exceed 255 characters");
    }

    [Fact]
    public void Validate_WithFutureDateOfBirth_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddDays(1) // Future date
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.DateOfBirth)
            .WithErrorMessage("Date of birth must be in the past");
    }

    [Fact]
    public void Validate_WithVeryOldDateOfBirth_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-151) // More than 150 years ago
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.DateOfBirth)
            .WithErrorMessage("Date of birth cannot be more than 150 years ago");
    }

    [Fact]
    public void Validate_WithInvalidPhone_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            Phone = "0555-invalid"
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Phone)
            .WithErrorMessage("Phone must be a valid phone number");
    }

    [Fact]
    public void Validate_WithValidPhoneFormats_ShouldNotHaveValidationErrors()
    {
        // Arrange & Act & Assert
        var validPhones = new[] { "5551234567", "+15551234567", "1234567890" };

        foreach (var phone in validPhones)
        {
            var dto = new UpdatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = DateTime.Today.AddYears(-30),
                Phone = phone
            };

            _validator.TestValidate(dto)
                .ShouldNotHaveValidationErrorFor(x => x.Phone);
        }
    }

    [Fact]
    public void Validate_WithTooLongGender_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            Gender = new string('M', 11) // 11 characters
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Gender)
            .WithErrorMessage("Gender cannot exceed 10 characters");
    }

    [Fact]
    public void Validate_WithTooLongAddress_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            Address = new string('A', 501) // 501 characters
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Address)
            .WithErrorMessage("Address cannot exceed 500 characters");
    }

    [Fact]
    public void Validate_WithTooLongEmergencyContact_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            EmergencyContact = new string('E', 201) // 201 characters
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.EmergencyContact)
            .WithErrorMessage("Emergency contact cannot exceed 200 characters");
    }

    [Fact]
    public void Validate_WithInvalidEmergencyPhone_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            EmergencyPhone = "0555-invalid"
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.EmergencyPhone)
            .WithErrorMessage("Emergency phone must be a valid phone number");
    }

    [Fact]
    public void Validate_WithTooLongMedicalHistory_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            MedicalHistory = new string('M', 2001) // 2001 characters
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.MedicalHistory)
            .WithErrorMessage("Medical history cannot exceed 2000 characters");
    }

    [Fact]
    public void Validate_WithTooLongAllergies_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            Allergies = new string('A', 1001) // 1001 characters
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Allergies)
            .WithErrorMessage("Allergies cannot exceed 1000 characters");
    }

    [Fact]
    public void Validate_WithNullOptionalFields_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            Phone = null,
            Gender = null,
            Address = null,
            EmergencyContact = null,
            EmergencyPhone = null,
            MedicalHistory = null,
            Allergies = null
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}