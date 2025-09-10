namespace JustBeeWeb.Models;

public class Alveole
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public required string Description { get; set; }
    public required string VilleCode { get; set; }
    public required string Email { get; set; }
    public bool EmailVerifie { get; set; } = false;
    public string? TokenVerification { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public DateTime? DateVerification { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Navigation property
    public Ville? Ville { get; set; }
}