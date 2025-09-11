using JustBeeWeb.Services;
using JustBeeWeb.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace JustBeeWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VillesController : ControllerBase
{
    private readonly VilleService _villeService;
    private readonly VilleDataService _villeDataService;
    private readonly ILogger<VillesController> _logger;

    public VillesController(VilleService villeService, VilleDataService villeDataService, ILogger<VillesController> logger)
    {
        _villeService = villeService;
        _villeDataService = villeDataService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVillesAsync()
    {
        var villes = await _villeService.GetAllVillesAsync();
        return Ok(villes);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetVilleByCodeAsync(string code)
    {
        var ville = await _villeService.GetVilleByCodeAsync(code);
        if (ville is null)
        {
            return NotFound($"Ville avec le code {code} non trouvée.");
        }
        return Ok(ville);
    }

    [HttpGet("with-persons")]
    public async Task<IActionResult> GetVillesWithPersonsAsync()
    {
        var allPersons = await _villeService.GetPersonsVerifieesAsync();
        var villesWithPersons = allPersons
            .Where(p => !string.IsNullOrEmpty(p.VilleCode))
            .GroupBy(p => p.VilleCode)
            .Select(g => g.Key)
            .ToList();

        var villes = new List<Ville>();
        foreach (var villeCode in villesWithPersons)
        {
            var ville = await _villeService.GetVilleByCodeAsync(villeCode!);
            if (ville != null)
                villes.Add(ville);
        }

        return Ok(villes);
    }

    [HttpGet("with-alveoles")]
    public async Task<IActionResult> GetVillesWithAlveolesAsync()
    {
        var alveoles = await _villeService.GetAlveolesVerifieesAsync();
        var villesWithAlveoles = alveoles
            .GroupBy(a => a.VilleCode)
            .Select(g => g.Key)
            .ToList();

        var villes = new List<Ville>();
        foreach (var villeCode in villesWithAlveoles)
        {
            var ville = await _villeService.GetVilleByCodeAsync(villeCode);
            if (ville != null)
                villes.Add(ville);
        }

        return Ok(villes);
    }

    [HttpPost("{code}/persons")]
    public async Task<IActionResult> AddPersonToVilleAsync(string code, [FromBody] CreatePersonRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Pseudo))
        {
            return BadRequest("Le pseudo est requis.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("L'email est requis.");
        }

        var ville = await _villeService.GetVilleByCodeAsync(code);
        if (ville is null)
        {
            return NotFound($"Ville avec le code {code} non trouvée.");
        }

        var person = new Person
        {
            Pseudo = request.Pseudo.Trim(),
            Email = request.Email.Trim().ToLower()
        };

        var success = await _villeService.AddPersonToVilleAsync(code, person);
        if (!success)
        {
            return BadRequest("Erreur lors de l'ajout de la personne.");
        }

        return CreatedAtAction(nameof(GetVilleByCodeAsync), new { code }, person);
    }

    [HttpPost("{code}/alveoles")]
    public async Task<IActionResult> AddAlveoleToVilleAsync(string code, [FromBody] CreateAlveoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nom))
        {
            return BadRequest("Le nom de l'alvéole est requis.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("L'email est requis.");
        }

        var ville = await _villeService.GetVilleByCodeAsync(code);
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

        var success = await _villeService.AddAlveoleToVilleAsync(code, alveole);
        if (!success)
        {
            return BadRequest("Erreur lors de l'ajout de l'alvéole.");
        }

        return CreatedAtAction(nameof(GetVilleByCodeAsync), new { code }, alveole);
    }

    [HttpDelete("{code}/persons/{personId}")]
    public async Task<IActionResult> RemovePersonFromVilleAsync(string code, int personId)
    {
        var removed = await _villeService.RemovePersonFromVilleAsync(code, personId);
        if (!removed)
        {
            return NotFound($"Personne avec l'ID {personId} non trouvée dans la ville {code}.");
        }

        return NoContent();
    }

    [HttpDelete("{code}/alveoles/{alveoleId}")]
    public async Task<IActionResult> RemoveAlveoleFromVilleAsync(string code, int alveoleId)
    {
        var removed = await _villeService.RemoveAlveoleFromVilleAsync(code, alveoleId);
        if (!removed)
        {
            return NotFound($"Alvéole avec l'ID {alveoleId} non trouvée dans la ville {code}.");
        }

        return NoContent();
    }

    [HttpGet("persons")]
    public async Task<IActionResult> GetAllPersonsAsync()
    {
        var persons = await _villeService.GetAllPersonsAsync();
        return Ok(persons);
    }

    [HttpGet("persons/verified")]
    public async Task<IActionResult> GetVerifiedPersonsAsync()
    {
        var persons = await _villeService.GetPersonsVerifieesAsync();
        return Ok(persons);
    }

    [HttpGet("alveoles")]
    public async Task<IActionResult> GetAllAlveolesAsync()
    {
        var alveoles = await _villeService.GetAllAlveolesAsync();
        return Ok(alveoles);
    }

    [HttpGet("alveoles/verified")]
    public async Task<IActionResult> GetVerifiedAlveolesAsync()
    {
        var alveoles = await _villeService.GetAlveolesVerifieesAsync();
        return Ok(alveoles);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync([FromQuery] string? q)
    {
        try
        {
            var villes = await _villeDataService.SearchVillesAsync(q ?? "");

            var result = villes.Select(v => new VilleSearchResult
            {
                Code = v.Code,
                Nom = v.Nom,
                Departement = v.Departement,
                Region = v.Region,
                Display = $"{v.Nom} ({v.Departement}, {v.Region})"
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiErrorResponse { Error = "Erreur lors de la recherche de villes", Details = ex.Message });
        }
    }

    [HttpGet("all-france")]
    public async Task<IActionResult> GetAllFranceAsync([FromQuery] int? limit = 100)
    {
        try
        {
            var villes = await _villeDataService.GetAllVillesFranceAsync();

            if (villes.Count == 0)
            {
                return Ok(new List<VilleSearchResult>());
            }

            // Appliquer la limite si spécifiée
            var limitedVilles = limit.HasValue ? villes.Take(limit.Value) : villes;
            
            var result = limitedVilles.Select(v => new VilleSearchResult
            {
                Code = v.Code,
                Nom = v.Nom,
                Departement = v.Departement,
                Region = v.Region,
                Display = $"{v.Nom} ({v.Departement}, {v.Region})"
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erreur lors du chargement des villes françaises");
            return StatusCode(500, new ApiErrorResponse { Error = "Erreur lors du chargement des villes", Details = ex.Message });
        }
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularVillesAsync()
    {
        try
        {
            // Retourner les villes populaires directement depuis le service
            var villes = await _villeDataService.GetAllVillesFranceAsync();
            
            // Prendre les 50 premières villes triées par nom (qui incluent les grandes villes)
            var popularVilles = villes.Take(50).Select(v => new VilleSearchResult
            {
                Code = v.Code,
                Nom = v.Nom,
                Departement = v.Departement,
                Region = v.Region,
                Display = $"{v.Nom} ({v.Departement}, {v.Region})"
            }).ToList();

            return Ok(popularVilles);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erreur lors du chargement des villes populaires");
            return StatusCode(500, new ApiErrorResponse { Error = "Erreur lors du chargement des villes populaires", Details = ex.Message });
        }
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetVillesCountAsync()
    {
        try
        {
            var count = await _villeDataService.GetTotalVillesCountAsync();
            return Ok(new VilleCountResponse { Count = count, Message = $"{count} villes françaises disponibles" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du comptage des villes");
            return StatusCode(500, new ApiErrorResponse { Error = "Erreur lors du comptage des villes", Details = ex.Message });
        }
    }
}