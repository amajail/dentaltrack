using DentalTrack.Application.DTOs;
using DentalTrack.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace DentalTrack.Application.Tests.Validators;

public class UpdateTreatmentDtoValidatorTests
{
    private readonly UpdateTreatmentDtoValidator _validator;

    public UpdateTreatmentDtoValidatorTests()
    {
        _validator = new UpdateTreatmentDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Teeth Whitening Treatment",
            Description = "Professional teeth whitening procedure",
            EstimatedCost = 500.00m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyTitle_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "",
            Description = "Some description",
            EstimatedCost = 300.00m
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required");
    }

    [Fact]
    public void Validate_WithWhitespaceTitle_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "   ",
            Description = "Some description",
            EstimatedCost = 300.00m
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
        var dto = new UpdateTreatmentDto
        {
            Title = new string('A', 201), // 201 characters
            Description = "Some description",
            EstimatedCost = 300.00m
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
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = new string('D', 1001), // 1001 characters
            EstimatedCost = 300.00m
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description cannot exceed 1000 characters");
    }

    [Fact]
    public void Validate_WithNegativeEstimatedCost_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = "Valid description",
            EstimatedCost = -100.00m
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.EstimatedCost)
            .WithErrorMessage("Estimated cost cannot be negative");
    }

    [Fact]
    public void Validate_WithTooHighEstimatedCost_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = "Valid description",
            EstimatedCost = 1000001.00m // Over $1,000,000
        };

        // Act & Assert
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.EstimatedCost)
            .WithErrorMessage("Estimated cost cannot exceed $1,000,000");
    }

    [Fact]
    public void Validate_WithZeroEstimatedCost_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = "Valid description",
            EstimatedCost = 0.00m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.EstimatedCost);
    }

    [Fact]
    public void Validate_WithMaxValidEstimatedCost_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = "Valid description",
            EstimatedCost = 1000000.00m // Exactly $1,000,000
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.EstimatedCost);
    }

    [Fact]
    public void Validate_WithNullOptionalFields_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = null,
            EstimatedCost = null
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyDescription_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = "",
            EstimatedCost = 250.00m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithMaxValidDescription_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = new string('D', 1000), // Exactly 1000 characters
            EstimatedCost = 250.00m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithMaxValidTitle_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = new string('T', 200), // Exactly 200 characters
            Description = "Valid description",
            EstimatedCost = 250.00m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(100.50)]
    [InlineData(999999.99)]
    public void Validate_WithValidEstimatedCostValues_ShouldNotHaveValidationErrors(decimal validCost)
    {
        // Arrange
        var dto = new UpdateTreatmentDto
        {
            Title = "Valid Title",
            Description = "Valid description",
            EstimatedCost = validCost
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.EstimatedCost);
    }
}