using DentalTrack.Application.DTOs;
using FluentValidation;

namespace DentalTrack.Application.Validators;

public class UpdateTreatmentDtoValidator : AbstractValidator<UpdateTreatmentDto>
{
    public UpdateTreatmentDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0).WithMessage("Estimated cost cannot be negative")
            .LessThanOrEqualTo(1000000).WithMessage("Estimated cost cannot exceed $1,000,000")
            .When(x => x.EstimatedCost.HasValue);
    }
}