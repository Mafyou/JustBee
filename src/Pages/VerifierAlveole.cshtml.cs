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
            Message = "?? Token de v�rification manquant ou invalide.";
            Success = false;
            return Page();
        }

        // R�cup�rer l'alv�ole par son token
        var alveole = await _alveoleService.GetAlveoleByTokenAsync(token);
        if (alveole == null)
        {
            Message = "? Token de v�rification invalide ou expir�.";
            Success = false;
            return Page();
        }

        if (alveole.EmailVerifie)
        {
            Message = "? Cette alv�ole est d�j� v�rifi�e et active sur la carte.";
            Success = true;
            AlveoleNom = alveole.Nom;
            VilleNom = alveole.Ville?.Nom;
            return Page();
        }

        // V�rifier l'alv�ole
        var verificationReussie = await _alveoleService.VerifierAlveoleAsync(token);
        
        if (verificationReussie)
        {
            Message = "?? F�licitations ! Votre alv�ole a �t� v�rifi�e avec succ�s et est maintenant visible sur la carte des ruches d�mocratiques.";
            Success = true;
            AlveoleNom = alveole.Nom;
            VilleNom = alveole.Ville?.Nom;
        }
        else
        {
            Message = "?? Erreur lors de la v�rification. Veuillez r�essayer ou contacter l'administrateur.";
            Success = false;
        }

        return Page();
    }
}