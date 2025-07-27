using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string? GoogleId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public User(
        string email,
        string firstName,
        string lastName,
        UserRole role,
        string? googleId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        Email = email;
        GoogleId = googleId;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        SetUpdatedAt();
    }

    public void UpdateRole(UserRole role)
    {
        Role = role;
        SetUpdatedAt();
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
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
    
    public bool IsDoctor() => Role == UserRole.Doctor;
    public bool IsAdmin() => Role == UserRole.Admin;
    public bool IsAssistant() => Role == UserRole.Assistant;
}