namespace DentalTrack.Domain.ValueObjects;

public enum AnalysisType
{
    CariesDetection = 1,
    PlaqueAnalysis = 2,
    GumHealthAssessment = 3,
    ToothAlignment = 4,
    ColorMatching = 5,
    QualityAssessment = 6,
    ProgressComparison = 7,
    AnomalyDetection = 8,
    TreatmentPlanning = 9,
    RiskAssessment = 10,
    Other = 99
}

public static class AnalysisTypeExtensions
{
    public static string GetDisplayName(this AnalysisType type)
    {
        return type switch
        {
            AnalysisType.CariesDetection => "Caries Detection",
            AnalysisType.PlaqueAnalysis => "Plaque Analysis",
            AnalysisType.GumHealthAssessment => "Gum Health Assessment",
            AnalysisType.ToothAlignment => "Tooth Alignment Analysis",
            AnalysisType.ColorMatching => "Color Matching",
            AnalysisType.QualityAssessment => "Image Quality Assessment",
            AnalysisType.ProgressComparison => "Progress Comparison",
            AnalysisType.AnomalyDetection => "Anomaly Detection",
            AnalysisType.TreatmentPlanning => "Treatment Planning",
            AnalysisType.RiskAssessment => "Risk Assessment",
            AnalysisType.Other => "Other Analysis",
            _ => "Unknown"
        };
    }

    public static string GetDescription(this AnalysisType type)
    {
        return type switch
        {
            AnalysisType.CariesDetection => "AI-powered detection of dental caries and cavities",
            AnalysisType.PlaqueAnalysis => "Analysis of plaque buildup and distribution",
            AnalysisType.GumHealthAssessment => "Evaluation of gum health and periodontal conditions",
            AnalysisType.ToothAlignment => "Assessment of tooth positioning and alignment",
            AnalysisType.ColorMatching => "Color analysis for restorative procedures",
            AnalysisType.QualityAssessment => "Automated image quality evaluation",
            AnalysisType.ProgressComparison => "Comparison of treatment progress over time",
            AnalysisType.AnomalyDetection => "Detection of unusual patterns or conditions",
            AnalysisType.TreatmentPlanning => "AI-assisted treatment planning recommendations",
            AnalysisType.RiskAssessment => "Risk factor analysis and predictions",
            AnalysisType.Other => "Custom or specialized analysis",
            _ => "Unknown analysis type"
        };
    }

    public static bool IsAIBased(this AnalysisType type)
    {
        return type switch
        {
            AnalysisType.CariesDetection => true,
            AnalysisType.PlaqueAnalysis => true,
            AnalysisType.GumHealthAssessment => true,
            AnalysisType.AnomalyDetection => true,
            AnalysisType.TreatmentPlanning => true,
            AnalysisType.RiskAssessment => true,
            _ => false
        };
    }
}