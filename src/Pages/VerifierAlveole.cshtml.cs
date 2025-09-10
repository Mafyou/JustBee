using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class VerifierAlveoleModel : PageModel
{
    private readonly AlveoleService _alveoleService;
    private readonly VilleService _villeService;

    public VerifierAlveoleModel(AlveoleService alveoleService, VilleService villeService)
    {
        _alveoleService = alveoleService;
        _villeService = villeService;
    }

    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    public string? AlveoleNom { get; set; }
    public string? VilleNom { get; set; }

    public async Task<IActionResult> OnGetAsync(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            Message = "?? Token de vérification manquant ou invalide.";
            Success = false;
            return Page();
        }

        // Récupérer l'alvéole par son token
        var alveole = await _alveoleService.GetAlveoleByTokenAsync(token);
        if (alveole == null)
        {
            Message = "? Token de vérification invalide ou expiré.";
            Success = false;
            return Page();
        }

        if (alveole.EmailVerifie)
        {
            Message = "? Cette alvéole est déjà vérifiée et active sur la carte.";
            Success = true;
            AlveoleNom = alveole.Nom;
            VilleNom = alveole.Ville?.Nom;
            return Page();
        }

        // Vérifier l'alvéole
        var verificationReussie = await _alveoleService.VerifierAlveoleAsync(token);
        
        if (verificationReussie)
        {
            Message = "?? Félicitations ! Votre alvéole a été vérifiée avec succès et est maintenant visible sur la carte des ruches démocratiques.";
            Success = true;
            AlveoleNom = alveole.Nom;
            VilleNom = alveole.Ville?.Nom;
        }
        else
        {
            Message = "?? Erreur lors de la vérification. Veuillez réessayer ou contacter l'administrateur.";
            Success = false;
        }

        return Page();
    }
}