using JustBeeWeb.Models;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class CreerAlveoleModel(VilleService villeService, EmailService emailService) : PageModel
{
    private readonly VilleService _villeService = villeService;
    private readonly EmailService _emailService = emailService;

    [BindProperty]
    public string NomAlveole { get; set; } = string.Empty;

    [BindProperty]
    public string Description { get; set; } = string.Empty;

    [BindProperty]
    public string VilleCode { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    public List<Ville> Villes { get; set; } = [];

    public async Task OnGetAsync()
    {
        // Charger les villes les plus populaires pour l'affichage initial
        Villes = _villeService.GetAllVilles().OrderBy(v => v.Nom).ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(); // Recharger les données
            return Page();
        }

        // Validation des données
        if (string.IsNullOrWhiteSpace(NomAlveole) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(VilleCode))
        {
            TempData["Error"] = "Tous les champs sont obligatoires.";
            await OnGetAsync();
            return Page();
        }

        // Vérifier que la ville existe (ceci va maintenant chercher dans toutes les villes de France)
        var ville = _villeService.GetVilleByCode(VilleCode);
        if (ville == null)
        {
            TempData["Error"] = "Ville sélectionnée invalide.";
            await OnGetAsync();
            return Page();
        }

        // Créer la nouvelle alvéole
        var nouvelleAlveole = new Alveole
        {
            Nom = NomAlveole.Trim(),
            Description = Description?.Trim() ?? string.Empty,
            VilleCode = VilleCode,
            Email = Email.Trim().ToLower(),
            TokenVerification = Guid.NewGuid().ToString()
        };

        // Ajouter l'alvéole à la ville
        _villeService.AddAlveoleToVille(VilleCode, nouvelleAlveole);

        // Envoyer l'email de vérification
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var emailEnvoye = await _emailService.EnvoyerEmailVerificationAlveoleAsync(
            nouvelleAlveole.Email,
            nouvelleAlveole.Nom,
            ville.Nom,
            nouvelleAlveole.TokenVerification!,
            baseUrl
        );

        if (emailEnvoye)
        {
            TempData["Success"] = $"🏠 Alvéole '{NomAlveole}' créée avec succès ! Un email de vérification a été envoyé à {Email}. Vérifiez votre boîte mail pour activer votre alvéole.";
        }
        else
        {
            TempData["Warning"] = $"🏠 Alvéole '{NomAlveole}' créée, mais l'email de vérification n'a pas pu être envoyé. Contactez l'administrateur.";
        }

        return RedirectToPage("/MapBee");
    }
}