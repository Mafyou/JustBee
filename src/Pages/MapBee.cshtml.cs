using JustBeeWeb.Options;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace JustBeeWeb.Pages;

public class MapBeeModel : PageModel
{
    private readonly VilleService _villeService;
    private readonly DepartementService _departementService;
    private readonly IOptions<BrevoOptions> _brevo;

    public MapBeeModel(VilleService villeService, DepartementService departementService, IOptions<BrevoOptions> brevo)
    {
        _villeService = villeService;
        _departementService = departementService;
        _brevo = brevo;
    }

    public List<Ville> Villes { get; set; } = [];
    public List<Person> AllPersons { get; set; } = [];
    public List<Alveole> AllAlveoles { get; set; } = [];
    public List<Departement> Departements { get; set; } = [];
    public Dictionary<string, List<Person>> PersonsByDepartement { get; set; } = [];

    public async Task OnGetAsync()
    {
        // Récupérer toutes les villes
        Villes = await _villeService.GetAllVillesAsync();
        
        // Récupérer les départements
        Departements = await _departementService.GetAllDepartementsAsync();

        // Collecter seulement les personnes et alvéoles vérifiées pour la carte
        AllPersons = await _villeService.GetPersonsVerifieesAsync();
        AllAlveoles = await _villeService.GetAlveolesVerifieesAsync();

        // Build the persons by departement dictionary for compatibility
        PersonsByDepartement = new Dictionary<string, List<Person>>();
        foreach (var departement in Departements)
        {
            var personsInDept = await _departementService.GetPersonsInDepartementAsync(departement.Code);
            PersonsByDepartement[departement.Code] = personsInDept;
        }
    }
}