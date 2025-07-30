using DentalTrack.Application.DTOs;
using FluentValidation;

namespace DentalTrack.Application.Validators;

public class CompleteTreatmentDtoValidator : AbstractValidator<CompleteTreatmentDto>
{
    public CompleteTreatmentDtoValidator()
    {
        RuleFor(x => x.ActualCost)
            .GreaterThanOrEqualTo(0).WithMessage("Actual cost cannot be negative")
            .LessThanOrEqualTo(1000000).WithMessage("Actual cost cannot exceed $1,000,000")
            .When(x => x.ActualCost.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}