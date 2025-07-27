using DentalTrack.Application.DTOs;
using FluentValidation;

namespace DentalTrack.Application.Validators;

public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
            .Matches(@"^[\+]?[1-9][\d]{0,15}$").WithMessage("Phone must be a valid phone number")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-150)).WithMessage("Date of birth cannot be more than 150 years ago");

        RuleFor(x => x.Gender)
            .MaximumLength(10).WithMessage("Gender cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.Gender));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.EmergencyContact)
            .MaximumLength(100).WithMessage("Emergency contact cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.EmergencyContact));

        RuleFor(x => x.EmergencyPhone)
            .MaximumLength(20).WithMessage("Emergency phone cannot exceed 20 characters")
            .Matches(@"^[\+]?[1-9][\d]{0,15}$").WithMessage("Emergency phone must be a valid phone number")
            .When(x => !string.IsNullOrEmpty(x.EmergencyPhone));

        RuleFor(x => x.MedicalHistory)
            .MaximumLength(2000).WithMessage("Medical history cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.MedicalHistory));

        RuleFor(x => x.Allergies)
            .MaximumLength(1000).WithMessage("Allergies cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Allergies));
    }
}