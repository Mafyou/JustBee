namespace JustBeeWeb.Models;

public class Person
{
    public int Id { get; set; }
    public required string Pseudo { get; set; }
    public required string Email { get; set; } // Email requis
    public bool EmailVerifie { get; set; } = false; // Statut de vérification
    public string? TokenVerification { get; set; } // Token pour vérification
    public DateTime? DateVerification { get; set; } // Date de vérification
    public string? VilleCode { get; set; } // Changé de DepartementCode vers VilleCode
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    
    // Propriété de compatibilité pour éviter les erreurs dans l'existant
    public string? DepartementCode
    {
        get => VilleCode;
        set => VilleCode = value;
    }
}
