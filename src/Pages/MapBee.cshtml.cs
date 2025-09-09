using JustBeeWeb.Models;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class MapBeeModel : PageModel
{
    private readonly DepartementService _departementService;

    public MapBeeModel()
    {
        _departementService = new DepartementService();
    }

    public List<Departement> Departements { get; set; } = [];
    public List<Person> AllPersons { get; set; } = [];

    public void OnGet()
    {
        // Récupérer tous les départements
        Departements = _departementService.GetAllDepartements();

        // Ajouter des personnes d'exemple seulement si aucune personne n'existe déjà
        if (!_departementService.GetAllPersons().Any())
        {
            SeedPersonsInDepartements();
        }

        // Collecter toutes les personnes pour la carte
        AllPersons = Departements.SelectMany(d => d.Persons).ToList();
    }

    private void SeedPersonsInDepartements()
    {
        // Ajouter des personnes dans Paris (75)
        _departementService.AddPersonToDepartement("75", new Person { Id = 1, Pseudo = "ParisUser1" });
        _departementService.AddPersonToDepartement("75", new Person { Id = 2, Pseudo = "ParisUser2" });

        // Ajouter des personnes dans le Nord (59)
        _departementService.AddPersonToDepartement("59", new Person { Id = 3, Pseudo = "NordUser1" });

        // Ajouter des personnes dans les Bouches-du-Rhône (13)
        _departementService.AddPersonToDepartement("13", new Person { Id = 4, Pseudo = "MarseilleUser1" });
        _departementService.AddPersonToDepartement("13", new Person { Id = 5, Pseudo = "MarseilleUser2" });

        // Ajouter des personnes dans la Gironde (33)
        _departementService.AddPersonToDepartement("33", new Person { Id = 6, Pseudo = "BordeauxUser1" });

        // Ajouter des personnes dans le Rhône (69)
        _departementService.AddPersonToDepartement("69", new Person { Id = 7, Pseudo = "LyonUser1" });
        _departementService.AddPersonToDepartement("69", new Person { Id = 8, Pseudo = "LyonUser2" });
        _departementService.AddPersonToDepartement("69", new Person { Id = 9, Pseudo = "LyonUser3" });

        // Ajouter des personnes en Haute-Garonne (31)
        _departementService.AddPersonToDepartement("31", new Person { Id = 10, Pseudo = "ToulouseUser1" });

        // Ajouter des personnes dans l'Hérault (34)
        _departementService.AddPersonToDepartement("34", new Person { Id = 11, Pseudo = "MontpellierUser1" });
    }
}