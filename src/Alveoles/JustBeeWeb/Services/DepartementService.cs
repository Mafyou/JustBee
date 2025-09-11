using JustBeeInfrastructure.Repositories;
using Microsoft.Extensions.Caching.Hybrid;

namespace JustBeeWeb.Services;

public class DepartementService(IDepartementRepository departementRepository, IPersonRepository personRepository,
    IVilleRepository villeRepository, HybridCache cache)
{
    private readonly IDepartementRepository _departementRepository = departementRepository;
    private readonly IPersonRepository _personRepository = personRepository;
    private readonly IVilleRepository _villeRepository = villeRepository;
    private readonly HybridCache _cache = cache;

    public async Task<List<Departement>> GetAllDepartementsAsync()
    {
        const string cacheKey = "AllDepartements";

        return await _cache.GetOrCreateAsync(
            cacheKey,
            async cancellationToken => (await _departementRepository.GetAllAsync()).ToList(),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(1), // Departements rarely change
                LocalCacheExpiration = TimeSpan.FromMinutes(30)
            }
        );
    }

    public async Task<Departement?> GetDepartementByCodeAsync(string code)
    {
        return await _departementRepository.GetByCodeAsync(code);
    }

    // Get persons in a departement by finding all cities in that departement and their persons
    public async Task<List<Person>> GetPersonsInDepartementAsync(string departementCode)
    {
        var cacheKey = $"PersonsInDept_{departementCode}";

        return await _cache.GetOrCreateAsync(
            cacheKey,
            async cancellationToken =>
            {
                var villes = await _villeRepository.GetAllAsync();
                var villesInDept = villes.Where(v => v.Departement == departementCode).ToList();

                var allPersons = new List<Person>();
                foreach (var ville in villesInDept)
                {
                    var persons = await _personRepository.GetByVilleCodeAsync(ville.Code);
                    allPersons.AddRange(persons);
                }

                return allPersons;
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(2)
            }
        );
    }

    // This method is now misleading since we're adding person to a ville, not departement
    // Consider deprecating this in favor of adding directly to a ville
    public async Task<bool> AddPersonToDepartementAsync(string departementCode, Person person)
    {
        var departement = await GetDepartementByCodeAsync(departementCode);
        if (departement is not null)
        {
            // Find a default ville in this departement (you might want to change this logic)
            var villes = await _villeRepository.GetAllAsync();
            var villeInDept = villes.FirstOrDefault(v => v.Departement == departementCode);

            if (villeInDept != null)
            {
                person.VilleCode = villeInDept.Code;
                person.Latitude = departement.Latitude;
                person.Longitude = departement.Longitude;

                await _personRepository.AddAsync(person);

                // Invalidate related cache entries
                await _cache.RemoveAsync($"PersonsInDept_{departementCode}");
                await _cache.RemoveAsync("MapBeeData");

                return true;
            }
        }
        return false;
    }

    public async Task<bool> RemovePersonFromDepartementAsync(string departementCode, int personId)
    {
        var person = await _personRepository.GetByIdAsync(personId);
        if (person?.VilleCode != null)
        {
            // Check if the person's ville is in the specified departement
            var ville = await _villeRepository.GetByCodeAsync(person.VilleCode);
            if (ville?.Departement == departementCode)
            {
                var result = await _personRepository.DeleteAsync(personId);

                if (result)
                {
                    // Invalidate related cache entries
                    await _cache.RemoveAsync($"PersonsInDept_{departementCode}");
                    await _cache.RemoveAsync("MapBeeData");
                }

                return result;
            }
        }
        return false;
    }

    public async Task<List<Person>> GetAllPersonsAsync() =>
        (await _personRepository.GetAllAsync()).ToList();

    public async Task<Person?> GetPersonByIdAsync(int id) =>
        await _personRepository.GetByIdAsync(id);
}