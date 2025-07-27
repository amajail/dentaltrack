using DentalTrack.Application.DTOs;
using DentalTrack.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace DentalTrack.Application.Tests.Validators;

public class CreatePatientDtoValidatorTests
{
    private readonly CreatePatientDtoValidator _validator;

    public CreatePatientDtoValidatorTests()
    {
        _validator = new CreatePatientDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CreatePatientDto
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
        var dto = new CreatePatientDto
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
        var dto = new CreatePatientDto
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
        var dto = new CreatePatientDto
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
    public void Validate_WithInvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreatePatientDto
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
        var dto = new CreatePatientDto
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
    public void Validate_WithFutureDateOfBirth_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreatePatientDto
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
        var dto = new CreatePatientDto
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
        var dto = new CreatePatientDto
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
            var dto = new CreatePatientDto
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
    public void Validate_WithNullOptionalFields_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CreatePatientDto
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