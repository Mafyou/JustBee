using JustBeeInfrastructure.Repositories;
using JustBeeInfrastructure.Models;

namespace JustBeeWeb.Services;

public class DepartementService(IDepartementRepository departementRepository, IPersonRepository personRepository, IVilleRepository villeRepository)
{
    private readonly IDepartementRepository _departementRepository = departementRepository;
    private readonly IPersonRepository _personRepository = personRepository;
    private readonly IVilleRepository _villeRepository = villeRepository;

    public async Task<List<Departement>> GetAllDepartementsAsync()
    {
        return (await _departementRepository.GetAllAsync()).ToList();
    }

    public List<Departement> GetAllDepartements() => 
        GetAllDepartementsAsync().Result;

    public async Task<Departement?> GetDepartementByCodeAsync(string code)
    {
        return await _departementRepository.GetByCodeAsync(code);
    }

    public Departement? GetDepartementByCode(string code) => 
        GetDepartementByCodeAsync(code).Result;

    // Get persons in a departement by finding all cities in that departement and their persons
    public async Task<List<Person>> GetPersonsInDepartementAsync(string departementCode)
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
    }

    public List<Person> GetPersonsInDepartement(string departementCode) => 
        GetPersonsInDepartementAsync(departementCode).Result;

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
                return true;
            }
        }
        return false;
    }

    public void AddPersonToDepartement(string departementCode, Person person) => 
        AddPersonToDepartementAsync(departementCode, person).Wait();

    public async Task<bool> RemovePersonFromDepartementAsync(string departementCode, int personId)
    {
        var person = await _personRepository.GetByIdAsync(personId);
        if (person?.VilleCode != null)
        {
            // Check if the person's ville is in the specified departement
            var ville = await _villeRepository.GetByCodeAsync(person.VilleCode);
            if (ville?.Departement == departementCode)
            {
                return await _personRepository.DeleteAsync(personId);
            }
        }
        return false;
    }

    public bool RemovePersonFromDepartement(string departementCode, int personId) => 
        RemovePersonFromDepartementAsync(departementCode, personId).Result;

    public async Task<List<Person>> GetAllPersonsAsync() =>
        (await _personRepository.GetAllAsync()).ToList();

    public List<Person> GetAllPersons() => GetAllPersonsAsync().Result;

    public async Task<Person?> GetPersonByIdAsync(int id) =>
        await _personRepository.GetByIdAsync(id);

    public Person? GetPersonById(int id) => GetPersonByIdAsync(id).Result;
}