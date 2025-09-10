using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class VerifierEmailModel : PageModel
{
    private readonly VilleService _villeService;

    public VerifierEmailModel(VilleService villeService)
    {
        _villeService = villeService;
    }

    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    public string? PersonPseudo { get; set; }
    public string? VilleNom { get; set; }

    public async Task<IActionResult> OnGetAsync(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            Message = "?? Token de v�rification manquant ou invalide.";
            Success = false;
            return Page();
        }

        // R�cup�rer la personne par son token
        var person = await _villeService.GetPersonByTokenAsync(token);
        if (person == null)
        {
            Message = "? Token de v�rification invalide ou expir�.";
            Success = false;
            return Page();
        }

        if (person.EmailVerifie)
        {
            Message = "? Votre email est d�j� v�rifi� et vous �tes visible sur la carte.";
            Success = true;
            PersonPseudo = person.Pseudo;
            var ville = await _villeService.GetVilleByCodeAsync(person.VilleCode!);
            VilleNom = ville?.Nom;
            return Page();
        }

        // V�rifier l'email de la personne
        var verificationReussie = await _villeService.VerifierEmailPersonAsync(token);
        
        if (verificationReussie)
        {
            Message = "?? F�licitations ! Votre email a �t� v�rifi� avec succ�s et vous �tes maintenant visible sur la carte des ruches d�mocratiques.";
            Success = true;
            PersonPseudo = person.Pseudo;
            var ville = await _villeService.GetVilleByCodeAsync(person.VilleCode!);
            VilleNom = ville?.Nom;
        }
        else
        {
            Message = "?? Erreur lors de la v�rification. Veuillez r�essayer ou contacter l'administrateur.";
            Success = false;
        }

        return Page();
    }
}