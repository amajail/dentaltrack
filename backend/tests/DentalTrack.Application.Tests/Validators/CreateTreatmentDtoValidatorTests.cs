using DentalTrack.Application.DTOs;
using DentalTrack.Application.Validators;
using DentalTrack.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace DentalTrack.Application.Tests.Validators;

public class CreateTreatmentDtoValidatorTests
{
    private readonly CreateTreatmentDtoValidator _validator;

    public CreateTreatmentDtoValidatorTests()
    {
        _validator = new CreateTreatmentDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = "Routine Dental Cleaning",
            Description = "Regular dental cleaning and examination",
            StartDate = DateTime.Today.AddDays(1),
            EstimatedCost = 150.00m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyPatientId_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.Empty,
            Type = TreatmentType.Cleaning,
            Title = "Routine Dental Cleaning",
            StartDate = DateTime.Today.AddDays(1)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.PatientId)
            .WithErrorMessage("Patient ID is required");
    }

    [Fact]
    public void Validate_WithInvalidTreatmentType_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = (TreatmentType)999, // Invalid enum value
            Title = "Routine Dental Cleaning",
            StartDate = DateTime.Today.AddDays(1)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Type)
            .WithErrorMessage("Treatment type must be a valid enum value");
    }

    [Fact]
    public void Validate_WithEmptyTitle_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = "",
            StartDate = DateTime.Today.AddDays(1)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required");
    }

    [Fact]
    public void Validate_WithTooLongTitle_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = new string('A', 201), // 201 characters
            StartDate = DateTime.Today.AddDays(1)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title cannot exceed 200 characters");
    }

    [Fact]
    public void Validate_WithTooLongDescription_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = "Valid Title",
            Description = new string('A', 1001), // 1001 characters
            StartDate = DateTime.Today.AddDays(1)
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description cannot exceed 1000 characters");
    }

    [Fact]
    public void Validate_WithTooOldStartDate_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = "Valid Title",
            StartDate = DateTime.Today.AddDays(-31) // More than 30 days ago
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.StartDate)
            .WithErrorMessage("Start date cannot be more than 30 days in the past");
    }

    [Fact]
    public void Validate_WithNegativeEstimatedCost_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = "Valid Title",
            StartDate = DateTime.Today.AddDays(1),
            EstimatedCost = -100.00m
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.EstimatedCost)
            .WithErrorMessage("Estimated cost must be greater than or equal to 0");
    }

    [Fact]
    public void Validate_WithTooHighEstimatedCost_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = "Valid Title",
            StartDate = DateTime.Today.AddDays(1),
            EstimatedCost = 1000000.00m // 1 million
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.EstimatedCost)
            .WithErrorMessage("Estimated cost must be less than 1,000,000");
    }

    [Fact]
    public void Validate_WithValidStartDateRange_ShouldNotHaveValidationErrors()
    {
        // Test various valid start dates
        var validDates = new[]
        {
            DateTime.Today.AddDays(-29), // 29 days ago (within 30 day limit)
            DateTime.Today,              // Today
            DateTime.Today.AddDays(1),   // Tomorrow
            DateTime.Today.AddMonths(1)  // Future date
        };

        foreach (var date in validDates)
        {
            var dto = new CreateTreatmentDto
            {
                PatientId = Guid.NewGuid(),
                Type = TreatmentType.Cleaning,
                Title = "Valid Title",
                StartDate = date
            };

            _validator.TestValidate(dto)
                .ShouldNotHaveValidationErrorFor(x => x.StartDate);
        }
    }

    [Fact]
    public void Validate_WithNullOptionalFields_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CreateTreatmentDto
        {
            PatientId = Guid.NewGuid(),
            Type = TreatmentType.Cleaning,
            Title = "Valid Title",
            StartDate = DateTime.Today.AddDays(1),
            Description = null,
            EstimatedCost = null
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}