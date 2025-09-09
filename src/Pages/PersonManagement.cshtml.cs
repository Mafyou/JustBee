using JustBeeWeb.Models;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustBeeWeb.Pages;

public class PersonManagementModel : PageModel
{
    private readonly DepartementService _departementService;

    public PersonManagementModel()
    {
        _departementService = new DepartementService();
    }

    public List<Departement> Departements { get; set; } = [];
    public List<Person> AllPersons { get; set; } = [];
    public string? SelectedDepartementCode { get; set; }

    [BindProperty]
    public string NewPersonPseudo { get; set; } = string.Empty;

    [BindProperty]
    public string NewPersonDepartement { get; set; } = string.Empty;

    public void OnGet(string? dept = null)
    {
        Departements = _departementService.GetAllDepartements();
        AllPersons = _departementService.GetAllPersons();
        SelectedDepartementCode = dept;
    }

    public IActionResult OnPostAddPerson()
    {
        if (string.IsNullOrWhiteSpace(NewPersonPseudo) || string.IsNullOrWhiteSpace(NewPersonDepartement))
        {
            TempData["Error"] = "Veuillez remplir tous les champs.";
            return RedirectToPage();
        }

        var departement = _departementService.GetDepartementByCode(NewPersonDepartement);
        if (departement == null)
        {
            TempData["Error"] = $"Département {NewPersonDepartement} non trouvé.";
            return RedirectToPage();
        }

        var person = new Person
        {
            Pseudo = NewPersonPseudo.Trim()
        };

        _departementService.AddPersonToDepartement(NewPersonDepartement, person);
        
        TempData["Success"] = $"Personne '{person.Pseudo}' ajoutée avec succès au département {departement.Nom}!";
        
        return RedirectToPage();
    }

    public IActionResult OnPostDeletePerson(int personId, string departementCode)
    {
        var removed = _departementService.RemovePersonFromDepartement(departementCode, personId);
        
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

    public IActionResult OnPostGenerateRandomPersons(int count = 5)
    {
        var random = new Random();
        var pseudos = new[] { "Alex", "Marie", "Pierre", "Sophie", "Jean", "Emma", "Lucas", "Camille", "Thomas", "Léa", 
                             "Antoine", "Clara", "Nicolas", "Manon", "Julien", "Chloé", "Maxime", "Laura", "Hugo", "Jade" };
        var departementCodes = new[] { "75", "69", "13", "31", "33", "59", "67", "06", "34", "44" };

        for (int i = 0; i < count; i++)
        {
            var pseudo = pseudos[random.Next(pseudos.Length)] + "_" + random.Next(1000, 9999);
            var deptCode = departementCodes[random.Next(departementCodes.Length)];

            var person = new Person { Pseudo = pseudo };
            _departementService.AddPersonToDepartement(deptCode, person);
        }

        TempData["Success"] = $"{count} personnes générées aléatoirement!";
        return RedirectToPage();
    }
}