namespace DentalTrack.Domain.ValueObjects;

public enum UserRole
{
    Doctor = 1,
    Assistant = 2,
    Admin = 3
}

public static class UserRoleExtensions
{
    public static string GetDisplayName(this UserRole role)
    {
        return role switch
        {
            UserRole.Doctor => "Doctor",
            UserRole.Assistant => "Assistant", 
            UserRole.Admin => "Admin",
            _ => "Unknown"
        };
    }

    public static bool CanManagePatients(this UserRole role)
    {
        return role == UserRole.Doctor || role == UserRole.Admin;
    }

    public static bool CanViewReports(this UserRole role)
    {
        return role == UserRole.Doctor || role == UserRole.Admin;
    }

    public static bool CanManageUsers(this UserRole role)
    {
        return role == UserRole.Admin;
    }

    public static bool CanPerformTreatments(this UserRole role)
    {
        return role == UserRole.Doctor;
    }
}