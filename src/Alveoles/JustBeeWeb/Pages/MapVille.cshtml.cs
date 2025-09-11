using JustBeeWeb.Options;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace JustBeeWeb.Pages;

public class MapVilleModel : PageModel
{
    private readonly VilleService _villeService;
    private readonly IOptions<BrevoOptions> _brevo;

    public MapVilleModel(VilleService villeService, IOptions<BrevoOptions> brevo)
    {
        _villeService = villeService;
        _brevo = brevo;
    }

    public List<Ville> Villes { get; set; } = [];
    public List<Person> AllPersons { get; set; } = [];

    public async Task OnGetAsync()
    {
        // Récupérer toutes les villes
        Villes = await _villeService.GetAllVillesAsync();

        // Collecter toutes les personnes pour la carte
        AllPersons = await _villeService.GetAllPersonsAsync();
    }
}