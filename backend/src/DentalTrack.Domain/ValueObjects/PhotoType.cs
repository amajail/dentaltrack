namespace DentalTrack.Domain.ValueObjects;

public enum PhotoType
{
    Intraoral = 1,
    Extraoral = 2,
    Xray = 3,
    Panoramic = 4,
    Bitewing = 5,
    Periapical = 6,
    Progress = 7,
    Before = 8,
    After = 9,
    Clinical = 10,
    Other = 99
}

public static class PhotoTypeExtensions
{
    public static string GetDisplayName(this PhotoType type)
    {
        return type switch
        {
            PhotoType.Intraoral => "Intraoral Photo",
            PhotoType.Extraoral => "Extraoral Photo",
            PhotoType.Xray => "X-Ray",
            PhotoType.Panoramic => "Panoramic X-Ray",
            PhotoType.Bitewing => "Bitewing X-Ray",
            PhotoType.Periapical => "Periapical X-Ray",
            PhotoType.Progress => "Progress Photo",
            PhotoType.Before => "Before Photo",
            PhotoType.After => "After Photo",
            PhotoType.Clinical => "Clinical Photo",
            PhotoType.Other => "Other",
            _ => "Unknown"
        };
    }

    public static bool IsXRay(this PhotoType type)
    {
        return type switch
        {
            PhotoType.Xray => true,
            PhotoType.Panoramic => true,
            PhotoType.Bitewing => true,
            PhotoType.Periapical => true,
            _ => false
        };
    }

    public static bool RequiresSpecialHandling(this PhotoType type)
    {
        return type.IsXRay();
    }
}