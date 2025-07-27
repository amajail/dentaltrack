using DentalTrack.Application.DTOs;
using FluentValidation;

namespace DentalTrack.Application.Validators;

public class CreateTreatmentDtoValidator : AbstractValidator<CreateTreatmentDto>
{
    public CreateTreatmentDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("Patient ID is required");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Treatment type must be a valid enum value");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today.AddDays(-30))
            .WithMessage("Start date cannot be more than 30 days in the past");

        RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0).WithMessage("Estimated cost must be greater than or equal to 0")
            .LessThan(1000000).WithMessage("Estimated cost must be less than 1,000,000")
            .When(x => x.EstimatedCost.HasValue);
    }
}