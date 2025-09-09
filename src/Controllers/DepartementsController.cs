using JustBeeWeb.Models;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace JustBeeWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartementsController : ControllerBase
{
    private readonly DepartementService _departementService;

    public DepartementsController()
    {
        _departementService = new DepartementService();
    }

    [HttpGet]
    public IActionResult GetAllDepartements()
    {
        var departements = _departementService.GetAllDepartements();
        return Ok(departements);
    }

    [HttpGet("{code}")]
    public IActionResult GetDepartementByCode(string code)
    {
        var departement = _departementService.GetDepartementByCode(code);
        if (departement == null)
        {
            return NotFound($"Département avec le code {code} non trouvé.");
        }
        return Ok(departement);
    }

    [HttpGet("with-persons")]
    public IActionResult GetDepartementsWithPersons()
    {
        var departements = _departementService.GetAllDepartements()
            .Where(d => d.Persons.Any())
            .ToList();
        return Ok(departements);
    }

    [HttpPost("{code}/persons")]
    public IActionResult AddPersonToDepartement(string code, [FromBody] CreatePersonRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Pseudo))
        {
            return BadRequest("Le pseudo est requis.");
        }

        var departement = _departementService.GetDepartementByCode(code);
        if (departement == null)
        {
            return NotFound($"Département avec le code {code} non trouvé.");
        }

        var person = new Person
        {
            Pseudo = request.Pseudo.Trim()
        };

        _departementService.AddPersonToDepartement(code, person);

        return CreatedAtAction(nameof(GetDepartementByCode), new { code }, person);
    }

    [HttpDelete("{code}/persons/{personId}")]
    public IActionResult RemovePersonFromDepartement(string code, int personId)
    {
        var removed = _departementService.RemovePersonFromDepartement(code, personId);
        if (!removed)
        {
            return NotFound($"Personne avec l'ID {personId} non trouvée dans le département {code}.");
        }

        return NoContent();
    }

    [HttpGet("persons")]
    public IActionResult GetAllPersons()
    {
        var persons = _departementService.GetAllPersons();
        return Ok(persons);
    }
}

public class CreatePersonRequest
{
    public string Pseudo { get; set; } = string.Empty;
}