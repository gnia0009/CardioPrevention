using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CardioPrevention.Models;

public class CardioPredictionRequest
{
    [Range(18, 120, ErrorMessage = "L'âge doit être compris entre 18 et 120 ans.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Veuillez sélectionner le sexe.")]
    [Range(0, 1)]
    public int Sex { get; set; }

    [Display(Name = "Pression artérielle au repos (mmHg)")]
    [Range(50, 300, ErrorMessage = "La pression doit être comprise entre 50 et 300 mmHg.")]
    public double Trestbps { get; set; }

    [Display(Name = "Cholestérol (mg/dL)")]
    [Range(50, 1000, ErrorMessage = "Le cholestérol doit être compris entre 50 et 1000 mg/dL.")]
    public double Chol { get; set; }

    [Display(Name = "Fréquence cardiaque maximale")]
    [Range(40, 260, ErrorMessage = "La fréquence cardiaque doit être comprise entre 40 et 260.")]
    public double Thalach { get; set; }

    [Display(Name = "Glycémie à jeun > 120 mg/dL")]
    [Range(0, 1)]
    public int Fbs { get; set; }

    [Display(Name = "Angine induite par l'effort")]
    [Range(0, 1)]
    public int Exang { get; set; }
}

public class CardioPredictionResponse
{
    [JsonPropertyName("probability")]
    public double Probability { get; set; }

    [JsonPropertyName("percentage")]
    public double Percentage { get; set; }

    [JsonPropertyName("risk_level")]
    public string RiskLevel { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("disclaimer")]
    public string Disclaimer { get; set; } = string.Empty;
}

public class CardioPredictionViewModel
{
    public CardioPredictionRequest Input { get; set; } = new();

    public CardioPredictionResponse? Result { get; set; }

    public string? ErrorMessage { get; set; }
}