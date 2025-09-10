using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class CreerAlveoleModel : PageModel
{
    private readonly VilleService _villeService;
    private readonly EmailService _emailService;
    private readonly AlveoleService _alveoleService;

    public CreerAlveoleModel(VilleService villeService, EmailService emailService, AlveoleService alveoleService)
    {
        _villeService = villeService;
        _emailService = emailService;
        _alveoleService = alveoleService;
    }

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
        Villes = await _villeService.GetAllVillesAsync();
        Villes = Villes.OrderBy(v => v.Nom).ToList();
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
        var ville = await _villeService.GetVilleByCodeAsync(VilleCode);
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
            Email = Email.Trim().ToLower()
        };

        // Ajouter l'alvéole via le service
        var success = await _alveoleService.AjouterAlveoleAsync(nouvelleAlveole);

        if (success)
        {
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
        }
        else
        {
            TempData["Error"] = "Erreur lors de la création de l'alvéole. Veuillez réessayer.";
            await OnGetAsync();
            return Page();
        }

        return RedirectToPage("/MapBee");
    }
}