using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class VerifierEmailModel : PageModel
{
    private readonly VilleService _villeService = new();

    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    public string? PersonPseudo { get; set; }
    public string? VilleNom { get; set; }

    public IActionResult OnGet(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            Message = "? Token de vérification manquant ou invalide.";
            Success = false;
            return Page();
        }

        // Récupérer la personne par son token
        var person = _villeService.GetPersonByToken(token);
        if (person == null)
        {
            Message = "? Token de vérification invalide ou expiré.";
            Success = false;
            return Page();
        }

        if (person.EmailVerifie)
        {
            Message = "?? Votre email est déjà vérifié et vous êtes visible sur la carte.";
            Success = true;
            PersonPseudo = person.Pseudo;
            var ville = _villeService.GetVilleByCode(person.VilleCode!);
            VilleNom = ville?.Nom;
            return Page();
        }

        // Vérifier l'email de la personne
        var verificationReussie = _villeService.VerifierEmailPerson(token);
        
        if (verificationReussie)
        {
            Message = "? Félicitations ! Votre email a été vérifié avec succès et vous êtes maintenant visible sur la carte des ruches démocratiques.";
            Success = true;
            PersonPseudo = person.Pseudo;
            var ville = _villeService.GetVilleByCode(person.VilleCode!);
            VilleNom = ville?.Nom;
        }
        else
        {
            Message = "? Erreur lors de la vérification. Veuillez réessayer ou contacter l'administrateur.";
            Success = false;
        }

        return Page();
    }
}