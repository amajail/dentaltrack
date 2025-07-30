using DentalTrack.Application.DTOs;
using DentalTrack.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace DentalTrack.Application.Tests.Validators;

public class CompleteTreatmentDtoValidatorTests
{
    private readonly CompleteTreatmentDtoValidator _validator;

    public CompleteTreatmentDtoValidatorTests()
    {
        _validator = new CompleteTreatmentDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 450.00m,
            Notes = "Treatment completed successfully with excellent results."
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithNegativeActualCost_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = -100.00m,
            Notes = "Some notes"
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.ActualCost)
            .WithErrorMessage("Actual cost cannot be negative");
    }

    [Fact]
    public void Validate_WithTooHighActualCost_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 1000001.00m, // Over $1,000,000
            Notes = "Some notes"
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.ActualCost)
            .WithErrorMessage("Actual cost cannot exceed $1,000,000");
    }

    [Fact]
    public void Validate_WithZeroActualCost_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 0.00m,
            Notes = "Free treatment"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ActualCost);
    }

    [Fact]
    public void Validate_WithMaxValidActualCost_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 1000000.00m, // Exactly $1,000,000
            Notes = "Expensive treatment"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ActualCost);
    }

    [Fact]
    public void Validate_WithTooLongNotes_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 500.00m,
            Notes = new string('N', 2001) // 2001 characters
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Notes)
            .WithErrorMessage("Notes cannot exceed 2000 characters");
    }

    [Fact]
    public void Validate_WithMaxValidNotes_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 500.00m,
            Notes = new string('N', 2000) // Exactly 2000 characters
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Validate_WithNullOptionalFields_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = null,
            Notes = null
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyNotes_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 300.00m,
            Notes = ""
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Validate_WithWhitespaceNotes_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 300.00m,
            Notes = "   "
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Notes);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(100.50)]
    [InlineData(999999.99)]
    [InlineData(500000.00)]
    public void Validate_WithValidActualCostValues_ShouldNotHaveValidationErrors(decimal validCost)
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = validCost,
            Notes = "Treatment completed"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ActualCost);
    }

    [Fact]
    public void Validate_WithOnlyActualCost_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 750.00m,
            Notes = null
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithOnlyNotes_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = null,
            Notes = "Treatment completed without additional cost tracking"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithBothFieldsProvided_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new CompleteTreatmentDto
        {
            ActualCost = 425.75m,
            Notes = "Successful completion with minor adjustments needed during procedure."
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}