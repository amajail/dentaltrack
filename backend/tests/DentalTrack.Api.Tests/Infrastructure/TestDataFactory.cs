using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Application.DTOs;

namespace DentalTrack.Api.Tests.Infrastructure;

public static class TestDataFactory
{
    public static Patient CreateTestPatient(string? email = null)
    {
        return new Patient(
            firstName: "John",
            lastName: "Doe",
            email: email ?? "john.doe@test.com",
            phone: "555-0123",
            dateOfBirth: new DateTime(1985, 5, 15),
            medicalHistory: "No known allergies"
        );
    }

    public static Treatment CreateTestTreatment(Guid? patientId = null)
    {
        return new Treatment(
            patientId: patientId ?? Guid.NewGuid(),
            type: TreatmentType.Whitening,
            title: "Teeth Whitening Treatment",
            description: "Initial whitening treatment",
            estimatedCost: 500m,
            startDate: DateTime.UtcNow
        );
    }

    public static User CreateTestUser(string? email = null)
    {
        return new User(
            email: email ?? "dentist@test.com",
            firstName: "Dr. Jane",
            lastName: "Smith",
            role: UserRole.Doctor
        );
    }

    public static PatientDto CreateTestPatientDto(string? email = null)
    {
        return new PatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email ?? "john.doe@test.com",
            Phone = "555-0123",
            DateOfBirth = new DateTime(1985, 5, 15),
            MedicalHistory = "No known allergies"
        };
    }

    public static CreateTreatmentDto CreateTestTreatmentDto(Guid? patientId = null)
    {
        return new CreateTreatmentDto
        {
            PatientId = patientId ?? Guid.NewGuid(),
            Title = "Teeth Whitening Treatment",
            Type = TreatmentType.Whitening,
            Description = "Initial whitening treatment",
            EstimatedCost = 500m,
            StartDate = DateTime.UtcNow
        };
    }
}