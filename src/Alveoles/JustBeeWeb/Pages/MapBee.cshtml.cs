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

    public MapBeeModel(VilleService villeService, DepartementService departementService,
        IOptions<BrevoOptions> brevo)
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
        // Execute sequentially to avoid DbContext concurrency issues
        Villes = await _villeService.GetAllVillesAsync();
        Departements = await _departementService.GetAllDepartementsAsync();
        AllPersons = await _villeService.GetPersonsVerifieesAsync();
        AllAlveoles = await _villeService.GetAlveolesVerifieesAsync();

        // Build the persons by departement dictionary efficiently
        PersonsByDepartement = [];

        // Group persons by departement in memory instead of multiple DB calls
        var personsByDept = AllPersons
            .Where(p => !string.IsNullOrEmpty(p.VilleCode))
            .Join(Villes, p => p.VilleCode, v => v.Code, (p, v) => new { Person = p, Ville = v })
            .GroupBy(x => x.Ville.Departement)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Person).ToList());

        foreach (var departement in Departements)
        {
            PersonsByDepartement[departement.Code] = personsByDept.GetValueOrDefault(departement.Code, []);
        }
    }
}