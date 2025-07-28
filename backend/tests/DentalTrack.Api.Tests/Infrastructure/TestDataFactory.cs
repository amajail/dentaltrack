using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Application.DTOs;

namespace DentalTrack.Api.Tests.Infrastructure;

public static class TestDataFactory
{
    public static Patient CreateTestPatient(string? email = null)
    {
        return new Patient
        {
            FirstName = "John",
            LastName = "Doe", 
            Email = email ?? "john.doe@test.com",
            Phone = "555-0123",
            DateOfBirth = new DateTime(1985, 5, 15),
            MedicalHistory = "No known allergies"
        };
    }

    public static Treatment CreateTestTreatment(Guid? patientId = null)
    {
        return new Treatment
        {
            PatientId = patientId ?? Guid.NewGuid(),
            Title = "Teeth Whitening Treatment",
            Type = TreatmentType.Professional,
            Status = TreatmentStatus.Active,
            StartDate = DateTime.UtcNow,
            ExpectedSessions = 3,
            CompletedSessions = 0,
            Notes = "Initial whitening treatment"
        };
    }

    public static User CreateTestUser(string? email = null)
    {
        return new User
        {
            Email = email ?? "dentist@test.com",
            FirstName = "Dr. Jane",
            LastName = "Smith",
            Role = UserRole.Dentist
        };
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

    public static TreatmentDto CreateTestTreatmentDto(Guid? patientId = null)
    {
        return new TreatmentDto
        {
            PatientId = patientId ?? Guid.NewGuid(),
            Title = "Teeth Whitening Treatment",
            Type = TreatmentType.Professional,
            Status = TreatmentStatus.Active,
            StartDate = DateTime.UtcNow,
            ExpectedSessions = 3,
            Notes = "Initial whitening treatment"
        };
    }
}