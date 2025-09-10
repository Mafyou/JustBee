using JustBeeWeb.Models;
using JustBeeWeb.Options;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace JustBeeWeb.Pages;

public class MapVilleModel(IOptions<BrevoOptions> brevo) : PageModel
{
    private readonly VilleService _villeService = new();
    private readonly IOptions<BrevoOptions> _brevo = brevo;

    public List<Ville> Villes { get; set; } = [];
    public List<Person> AllPersons { get; set; } = [];

    public void OnGet()
    {
        // Récupérer toutes les villes
        Villes = _villeService.GetAllVilles();

        // Ajouter des personnes d'exemple seulement si aucune personne n'existe déjà
        if (_villeService.GetAllPersons().Count == 0)
        {
            SeedPersonsInVilles();
        }

        // Collecter toutes les personnes pour la carte
        AllPersons = [.. Villes.SelectMany(v => v.Persons)];
    }

    private void SeedPersonsInVilles()
    {
        // Ajouter des personnes dans Paris
        _villeService.AddPersonToVille("PARIS", new Person { Id = 1, Pseudo = "ParisUser1", Email = "paris1@demo.fr", EmailVerifie = true });
        _villeService.AddPersonToVille("PARIS", new Person { Id = 2, Pseudo = "ParisUser2", Email = "paris2@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Lille
        _villeService.AddPersonToVille("LILLE", new Person { Id = 3, Pseudo = "LilleUser1", Email = "lille1@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Marseille
        _villeService.AddPersonToVille("MARSEILLE", new Person { Id = 4, Pseudo = "MarseilleUser1", Email = "marseille1@demo.fr", EmailVerifie = true });
        _villeService.AddPersonToVille("MARSEILLE", new Person { Id = 5, Pseudo = "MarseilleUser2", Email = "marseille2@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Bordeaux
        _villeService.AddPersonToVille("BORDEAUX", new Person { Id = 6, Pseudo = "BordeauxUser1", Email = "bordeaux1@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Lyon
        _villeService.AddPersonToVille("LYON", new Person { Id = 7, Pseudo = "LyonUser1", Email = "lyon1@demo.fr", EmailVerifie = true });
        _villeService.AddPersonToVille("LYON", new Person { Id = 8, Pseudo = "LyonUser2", Email = "lyon2@demo.fr", EmailVerifie = true });
        _villeService.AddPersonToVille("LYON", new Person { Id = 9, Pseudo = "LyonUser3", Email = "lyon3@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Toulouse
        _villeService.AddPersonToVille("TOULOUSE", new Person { Id = 10, Pseudo = "ToulouseUser1", Email = "toulouse1@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Montpellier
        _villeService.AddPersonToVille("MONTPELLIER", new Person { Id = 11, Pseudo = "MontpellierUser1", Email = "montpellier1@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Nice
        _villeService.AddPersonToVille("NICE", new Person { Id = 12, Pseudo = "NiceUser1", Email = "nice1@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Nantes
        _villeService.AddPersonToVille("NANTES", new Person { Id = 13, Pseudo = "NantesUser1", Email = "nantes1@demo.fr", EmailVerifie = true });
        _villeService.AddPersonToVille("NANTES", new Person { Id = 14, Pseudo = "NantesUser2", Email = "nantes2@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Strasbourg
        _villeService.AddPersonToVille("STRASBOURG", new Person { Id = 15, Pseudo = "StrasbourgUser1", Email = "strasbourg1@demo.fr", EmailVerifie = true });

        // Ajouter des personnes dans Rennes
        _villeService.AddPersonToVille("RENNES", new Person { Id = 16, Pseudo = "RennesUser1", Email = "rennes1@demo.fr", EmailVerifie = true });
    }
}