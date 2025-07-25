namespace DentalTrack.Domain.Entities;

public class Patient : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? Address { get; private set; }
    public string? EmergencyContact { get; private set; }
    public string? EmergencyPhone { get; private set; }
    public string? MedicalHistory { get; private set; }
    public string? Allergies { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    public ICollection<Treatment> Treatments { get; private set; } = new List<Treatment>();

    private Patient() { }

    public Patient(
        string firstName,
        string lastName,
        string email,
        DateTime dateOfBirth,
        string? phone = null,
        string? address = null,
        string? emergencyContact = null,
        string? emergencyPhone = null,
        string? medicalHistory = null,
        string? allergies = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        if (dateOfBirth >= DateTime.Today)
            throw new ArgumentException("Date of birth must be in the past", nameof(dateOfBirth));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        Phone = phone;
        Address = address;
        EmergencyContact = emergencyContact;
        EmergencyPhone = emergencyPhone;
        MedicalHistory = medicalHistory;
        Allergies = allergies;
    }

    public void UpdatePersonalInfo(
        string firstName,
        string lastName,
        string email,
        DateTime dateOfBirth,
        string? phone = null,
        string? address = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        Phone = phone;
        Address = address;
        SetUpdatedAt();
    }

    public void UpdateMedicalInfo(
        string? emergencyContact = null,
        string? emergencyPhone = null,
        string? medicalHistory = null,
        string? allergies = null)
    {
        EmergencyContact = emergencyContact;
        EmergencyPhone = emergencyPhone;
        MedicalHistory = medicalHistory;
        Allergies = allergies;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }

    public string GetFullName() => $"{FirstName} {LastName}";
    
    public int GetAge() => DateTime.Today.Year - DateOfBirth.Year - 
        (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
}