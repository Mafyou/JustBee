using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class PersonManagementModel(DepartementService departementService) : PageModel
{
    private readonly DepartementService _departementService = departementService;

    public List<Departement> Departements { get; set; } = [];
    public List<Person> AllPersons { get; set; } = [];
    public Dictionary<string, List<Person>> PersonsByDepartement { get; set; } = [];
    public string? SelectedDepartementCode { get; set; }

    [BindProperty]
    public string NewPersonPseudo { get; set; } = string.Empty;

    [BindProperty]
    public string NewPersonDepartement { get; set; } = string.Empty;

    public async Task OnGetAsync(string? dept = null)
    {
        Departements = await _departementService.GetAllDepartementsAsync();
        AllPersons = await _departementService.GetAllPersonsAsync();
        SelectedDepartementCode = dept;

        // Build the persons by departement dictionary
        PersonsByDepartement = new Dictionary<string, List<Person>>();
        foreach (var departement in Departements)
        {
            var personsInDept = await _departementService.GetPersonsInDepartementAsync(departement.Code);
            PersonsByDepartement[departement.Code] = personsInDept;
        }
    }

    public async Task<IActionResult> OnPostAddPersonAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPersonPseudo) || string.IsNullOrWhiteSpace(NewPersonDepartement))
        {
            TempData["Error"] = "Veuillez remplir tous les champs.";
            return RedirectToPage();
        }

        var departement = await _departementService.GetDepartementByCodeAsync(NewPersonDepartement);
        if (departement == null)
        {
            TempData["Error"] = $"Département {NewPersonDepartement} non trouvé.";
            return RedirectToPage();
        }

        var person = new Person
        {
            Pseudo = NewPersonPseudo.Trim(),
            Email = $"{NewPersonPseudo.Trim().ToLower()}@demo.fr"
        };

        var success = await _departementService.AddPersonToDepartementAsync(NewPersonDepartement, person);

        if (success)
        {
            TempData["Success"] = $"Personne '{person.Pseudo}' ajoutée avec succès au département {departement.Nom}!";
        }
        else
        {
            TempData["Error"] = "Erreur lors de l'ajout de la personne.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeletePersonAsync(int personId, string departementCode)
    {
        var removed = await _departementService.RemovePersonFromDepartementAsync(departementCode, personId);

        if (removed)
        {
            TempData["Success"] = "Personne supprimée avec succès!";
        }
        else
        {
            TempData["Error"] = "Impossible de supprimer la personne.";
        }

        return RedirectToPage();
    }
}