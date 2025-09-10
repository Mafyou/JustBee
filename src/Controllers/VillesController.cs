using JustBeeWeb.Models;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace JustBeeWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VillesController(VilleDataService villeDataService) : ControllerBase
{
    private readonly VilleService _villeService = new();
    private readonly VilleDataService _villeDataService = villeDataService;

    [HttpGet]
    public IActionResult GetAllVilles()
    {
        var villes = _villeService.GetAllVilles();
        return Ok(villes);
    }

    [HttpGet("{code}")]
    public IActionResult GetVilleByCode(string code)
    {
        var ville = _villeService.GetVilleByCode(code);
        if (ville is null)
        {
            return NotFound($"Ville avec le code {code} non trouvée.");
        }
        return Ok(ville);
    }

    [HttpGet("with-persons")]
    public IActionResult GetVillesWithPersons()
    {
        var villes = _villeService.GetAllVilles()
            .Where(v => v.Persons.Any(p => p.EmailVerifie))
            .ToList();
        return Ok(villes);
    }

    [HttpGet("with-alveoles")]
    public IActionResult GetVillesWithAlveoles()
    {
        var villes = _villeService.GetAllVilles()
            .Where(v => v.Alveoles.Any(a => a.EmailVerifie))
            .ToList();
        return Ok(villes);
    }

    [HttpPost("{code}/persons")]
    public IActionResult AddPersonToVille(string code, [FromBody] CreatePersonRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Pseudo))
        {
            return BadRequest("Le pseudo est requis.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("L'email est requis.");
        }

        var ville = _villeService.GetVilleByCode(code);
        if (ville is null)
        {
            return NotFound($"Ville avec le code {code} non trouvée.");
        }

        var person = new Person
        {
            Pseudo = request.Pseudo.Trim(),
            Email = request.Email.Trim().ToLower()
        };

        _villeService.AddPersonToVille(code, person);

        return CreatedAtAction(nameof(GetVilleByCode), new { code }, person);
    }

    [HttpPost("{code}/alveoles")]
    public IActionResult AddAlveoleToVille(string code, [FromBody] CreateAlveoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nom))
        {
            return BadRequest("Le nom de l'alvéole est requis.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("L'email est requis.");
        }

        var ville = _villeService.GetVilleByCode(code);
        if (ville is null)
        {
            return NotFound($"Ville avec le code {code} non trouvée.");
        }

        var alveole = new Alveole
        {
            Nom = request.Nom.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Email = request.Email.Trim().ToLower(),
            VilleCode = code
        };

        _villeService.AddAlveoleToVille(code, alveole);

        return CreatedAtAction(nameof(GetVilleByCode), new { code }, alveole);
    }

    [HttpDelete("{code}/persons/{personId}")]
    public IActionResult RemovePersonFromVille(string code, int personId)
    {
        var removed = _villeService.RemovePersonFromVille(code, personId);
        if (!removed)
        {
            return NotFound($"Personne avec l'ID {personId} non trouvée dans la ville {code}.");
        }

        return NoContent();
    }

    [HttpDelete("{code}/alveoles/{alveoleId}")]
    public IActionResult RemoveAlveoleFromVille(string code, int alveoleId)
    {
        var removed = _villeService.RemoveAlveoleFromVille(code, alveoleId);
        if (!removed)
        {
            return NotFound($"Alvéole avec l'ID {alveoleId} non trouvée dans la ville {code}.");
        }

        return NoContent();
    }

    [HttpGet("persons")]
    public IActionResult GetAllPersons()
    {
        var persons = _villeService.GetAllPersons();
        return Ok(persons);
    }

    [HttpGet("persons/verified")]
    public IActionResult GetVerifiedPersons()
    {
        var persons = _villeService.GetPersonsVerifiees();
        return Ok(persons);
    }

    [HttpGet("alveoles")]
    public IActionResult GetAllAlveoles()
    {
        var alveoles = _villeService.GetAllAlveoles();
        return Ok(alveoles);
    }

    [HttpGet("alveoles/verified")]
    public IActionResult GetVerifiedAlveoles()
    {
        var alveoles = _villeService.GetAlveolesVerifiees();
        return Ok(alveoles);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        try
        {
            var villes = await _villeDataService.SearchVillesAsync(q ?? "");

            var result = villes.Select(v => new
            {
                code = v.Code,
                nom = v.Nom,
                departement = v.Departement,
                region = v.Region,
                display = $"{v.Nom} ({v.Departement}, {v.Region})"
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erreur lors de la recherche de villes", details = ex.Message });
        }
    }

    [HttpGet("all-france")]
    public async Task<IActionResult> GetAllFrance()
    {
        try
        {
            var villes = await _villeDataService.GetAllVillesFranceAsync();

            var result = villes.Take(100).Select(v => new // Limiter pour éviter les réponses trop lourdes
            {
                code = v.Code,
                nom = v.Nom,
                departement = v.Departement,
                region = v.Region,
                display = $"{v.Nom} ({v.Departement}, {v.Region})"
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erreur lors du chargement des villes", details = ex.Message });
        }
    }
}

public class CreatePersonRequest
{
    public required string Pseudo { get; set; }
    public required string Email { get; set; }
}

public class CreateAlveoleRequest
{
    public required string Nom { get; set; }
    public string? Description { get; set; }
    public required string Email { get; set; }
}