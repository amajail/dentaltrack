namespace DentalTrack.Domain.ValueObjects;

public enum TreatmentType
{
    Consultation = 1,
    Cleaning = 2,
    Filling = 3,
    RootCanal = 4,
    Crown = 5,
    Bridge = 6,
    Implant = 7,
    Extraction = 8,
    Orthodontics = 9,
    Whitening = 10,
    Periodontal = 11,
    OralSurgery = 12,
    Emergency = 13,
    Other = 99
}

public static class TreatmentTypeExtensions
{
    public static string GetDisplayName(this TreatmentType type)
    {
        return type switch
        {
            TreatmentType.Consultation => "Consultation",
            TreatmentType.Cleaning => "Dental Cleaning",
            TreatmentType.Filling => "Filling",
            TreatmentType.RootCanal => "Root Canal",
            TreatmentType.Crown => "Crown",
            TreatmentType.Bridge => "Bridge",
            TreatmentType.Implant => "Dental Implant",
            TreatmentType.Extraction => "Tooth Extraction",
            TreatmentType.Orthodontics => "Orthodontic Treatment",
            TreatmentType.Whitening => "Teeth Whitening",
            TreatmentType.Periodontal => "Periodontal Treatment",
            TreatmentType.OralSurgery => "Oral Surgery",
            TreatmentType.Emergency => "Emergency Treatment",
            TreatmentType.Other => "Other",
            _ => "Unknown"
        };
    }

    public static bool RequiresMultipleSessions(this TreatmentType type)
    {
        return type switch
        {
            TreatmentType.RootCanal => true,
            TreatmentType.Orthodontics => true,
            TreatmentType.Periodontal => true,
            TreatmentType.Implant => true,
            _ => false
        };
    }
}