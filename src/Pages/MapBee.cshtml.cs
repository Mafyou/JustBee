using JustBeeWeb.Options;
using JustBeeWeb.Services;
using JustBeeWeb.Models;
using JustBeeWeb.Serialization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace JustBeeWeb.Pages;

public class MapBeeModel : PageModel
{
    private readonly VilleService _villeService;
    private readonly DepartementService _departementService;
    private readonly IOptions<BrevoOptions> _brevo;
    private readonly HybridCache _cache;

    public MapBeeModel(VilleService villeService, DepartementService departementService,
        IOptions<BrevoOptions> brevo, HybridCache cache)
    {
        _villeService = villeService;
        _departementService = departementService;
        _brevo = brevo;
        _cache = cache;
    }

    public List<Ville> Villes { get; set; } = [];
    public List<Person> AllPersons { get; set; } = [];
    public List<Alveole> AllAlveoles { get; set; } = [];
    public List<Departement> Departements { get; set; } = [];
    public Dictionary<string, List<Person>> PersonsByDepartement { get; set; } = [];

    public async Task OnGetAsync()
    {
        // Use HybridCache with stampede protection
        const string cacheKey = "MapBeeData";

        var cachedData = await _cache.GetOrCreateAsync(
            cacheKey,
            async cancellationToken =>
            {
                // Execute sequentially to avoid DbContext concurrency issues
                var villes = await _villeService.GetAllVillesAsync();
                var departements = await _departementService.GetAllDepartementsAsync();
                var allPersons = await _villeService.GetPersonsVerifieesAsync();
                var allAlveoles = await _villeService.GetAlveolesVerifieesAsync();

                // Convert to cache-safe DTOs
                var villesDto = villes.Select(v => new Serialization.VilleCacheDto
                {
                    Code = v.Code,
                    Nom = v.Nom,
                    Region = v.Region,
                    Departement = v.Departement,
                    Latitude = v.Latitude,
                    Longitude = v.Longitude
                }).ToList();

                var departementsDto = departements.Select(d => new Serialization.DepartementCacheDto
                {
                    Code = d.Code,
                    Nom = d.Nom,
                    Region = d.Region,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude
                }).ToList();

                var allPersonsDto = allPersons.Select(p => new Serialization.PersonCacheDto
                {
                    Id = p.Id,
                    Pseudo = p.Pseudo,
                    Email = p.Email,
                    EmailVerifie = p.EmailVerifie,
                    VilleCode = p.VilleCode,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    DateCreation = p.DateCreation,
                    DateVerification = p.DateVerification,
                    TokenVerification = p.TokenVerification
                }).ToList();

                var allAlveolesDto = allAlveoles.Select(a => new Serialization.AlveoleCacheDto
                {
                    Id = a.Id,
                    Nom = a.Nom,
                    Description = a.Description,
                    VilleCode = a.VilleCode,
                    Email = a.Email,
                    EmailVerifie = a.EmailVerifie,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    DateCreation = a.DateCreation,
                    DateVerification = a.DateVerification,
                    TokenVerification = a.TokenVerification
                }).ToList();

                // Build the persons by departement dictionary efficiently
                var personsByDepartement = new Dictionary<string, List<Serialization.PersonCacheDto>>();
                
                // Group persons by departement in memory instead of multiple DB calls
                var personsByDept = allPersonsDto
                    .Where(p => !string.IsNullOrEmpty(p.VilleCode))
                    .Join(villesDto, p => p.VilleCode, v => v.Code, (p, v) => new { Person = p, Ville = v })
                    .GroupBy(x => x.Ville.Departement)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Person).ToList());

                foreach (var departement in departementsDto)
                {
                    personsByDepartement[departement.Code] = personsByDept.GetValueOrDefault(departement.Code, new List<Serialization.PersonCacheDto>());
                }

                return new MapBeeData
                {
                    Villes = villesDto,
                    AllPersons = allPersonsDto,
                    AllAlveoles = allAlveolesDto,
                    Departements = departementsDto,
                    PersonsByDepartement = personsByDepartement
                };
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(2)
            }
        );

        // Convert DTOs back to domain objects for the view
        Villes = cachedData.Villes.Select(v => new Ville
        {
            Code = v.Code,
            Nom = v.Nom,
            Region = v.Region,
            Departement = v.Departement,
            Latitude = v.Latitude,
            Longitude = v.Longitude,
            Persons = [],
            Alveoles = []
        }).ToList();

        AllPersons = cachedData.AllPersons.Select(p => new Person
        {
            Id = p.Id,
            Pseudo = p.Pseudo,
            Email = p.Email,
            EmailVerifie = p.EmailVerifie,
            VilleCode = p.VilleCode,
            Latitude = p.Latitude,
            Longitude = p.Longitude,
            DateCreation = p.DateCreation,
            DateVerification = p.DateVerification,
            TokenVerification = p.TokenVerification
        }).ToList();

        AllAlveoles = cachedData.AllAlveoles.Select(a => new Alveole
        {
            Id = a.Id,
            Nom = a.Nom,
            Description = a.Description,
            VilleCode = a.VilleCode,
            Email = a.Email,
            EmailVerifie = a.EmailVerifie,
            Latitude = a.Latitude,
            Longitude = a.Longitude,
            DateCreation = a.DateCreation,
            DateVerification = a.DateVerification,
            TokenVerification = a.TokenVerification
        }).ToList();

        Departements = cachedData.Departements.Select(d => new Departement
        {
            Code = d.Code,
            Nom = d.Nom,
            Region = d.Region,
            Latitude = d.Latitude,
            Longitude = d.Longitude
        }).ToList();

        PersonsByDepartement = cachedData.PersonsByDepartement.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select(p => new Person
            {
                Id = p.Id,
                Pseudo = p.Pseudo,
                Email = p.Email,
                EmailVerifie = p.EmailVerifie,
                VilleCode = p.VilleCode,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                DateCreation = p.DateCreation,
                DateVerification = p.DateVerification,
                TokenVerification = p.TokenVerification
            }).ToList()
        );
    }
}