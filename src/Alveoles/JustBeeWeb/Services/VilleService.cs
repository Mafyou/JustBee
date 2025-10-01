using JustBeeInfrastructure.Repositories;

namespace JustBeeWeb.Services;

public class VilleService(
    IVilleRepository villeRepository,
    IPersonRepository personRepository,
    IAlveoleRepository alveoleRepository,
    VilleDataService? villeDataService = null)
{
    private readonly IVilleRepository _villeRepository = villeRepository;
    private readonly IPersonRepository _personRepository = personRepository;
    private readonly IAlveoleRepository _alveoleRepository = alveoleRepository;
    private readonly VilleDataService? _villeDataService = villeDataService;

    public async Task<List<Ville>> GetAllVillesAsync() =>
        [.. await _villeRepository.GetAllAsync()];

    public async Task<List<Ville>> GetAllVilles() =>
        await GetAllVillesAsync();

    public async Task<List<Ville>> GetAllVillesFranceAsync()
    {
        if (_villeDataService is not null)
        {
            return await _villeDataService.GetAllVillesFranceAsync();
        }
        return await GetAllVillesAsync();
    }

    public async Task<List<Ville>> SearchVillesAsync(string searchTerm)
    {
        if (_villeDataService is not null)
        {
            return await _villeDataService.SearchVillesAsync(searchTerm);
        }

        return [.. await _villeRepository.SearchAsync(searchTerm)];
    }

    public async Task<Ville?> GetVilleByCodeAsync(string code)
    {
        // Chercher d'abord dans la base de données
        var ville = await _villeRepository.GetByCodeAsync(code);
        if (ville is not null)
            return ville;

        // Si pas trouvé et qu'on a le service de données France, créer une ville
        if (_villeDataService is not null)
        {
            var villesFrance = await _villeDataService.GetAllVillesFranceAsync();
            var villeFrance = villesFrance.FirstOrDefault(v => v.Code == code);
            if (villeFrance is not null)
            {
                return await _villeRepository.AddAsync(villeFrance);
            }
        }

        return null;
    }

    public Ville? GetVilleByCode(string code) => GetVilleByCodeAsync(code).Result;

    public async Task<bool> AddPersonToVilleAsync(string villeCode, Person person)
    {
        var ville = await GetVilleByCodeAsync(villeCode);
        if (ville is not null)
        {
            // Générer un token de vérification si l'email n'est pas encore vérifié
            if (!person.EmailVerifie && string.IsNullOrEmpty(person.TokenVerification))
            {
                person.TokenVerification = $"{Guid.NewGuid()}";
            }

            person.VilleCode = villeCode;
            person.Latitude = ville.Latitude;
            person.Longitude = ville.Longitude;

            await _personRepository.AddAsync(person);
            return true;
        }
        return false;
    }

    public void AddPersonToVille(string villeCode, Person person) =>
        AddPersonToVilleAsync(villeCode, person).Wait();

    public async Task<bool> AddAlveoleToVilleAsync(string villeCode, Alveole alveole)
    {
        var ville = await GetVilleByCodeAsync(villeCode);
        if (ville is not null)
        {
            alveole.VilleCode = villeCode;
            alveole.Latitude = ville.Latitude;
            alveole.Longitude = ville.Longitude;

            await _alveoleRepository.AddAsync(alveole);
            return true;
        }
        return false;
    }

    public void AddAlveoleToVille(string villeCode, Alveole alveole) =>
        AddAlveoleToVilleAsync(villeCode, alveole).Wait();

    public async Task<bool> RemovePersonFromVilleAsync(string villeCode, int personId)
    {
        var person = await _personRepository.GetByIdAsync(personId);
        if (person?.VilleCode == villeCode)
        {
            return await _personRepository.DeleteAsync(personId);
        }
        return false;
    }

    public bool RemovePersonFromVille(string villeCode, int personId) =>
        RemovePersonFromVilleAsync(villeCode, personId).Result;

    public async Task<bool> RemoveAlveoleFromVilleAsync(string villeCode, int alveoleId)
    {
        var alveole = await _alveoleRepository.GetByIdAsync(alveoleId);
        if (alveole?.VilleCode == villeCode)
        {
            return await _alveoleRepository.DeleteAsync(alveoleId);
        }
        return false;
    }

    public bool RemoveAlveoleFromVille(string villeCode, int alveoleId) =>
        RemoveAlveoleFromVilleAsync(villeCode, alveoleId).Result;

    public async Task<List<Person>> GetAllPersonsAsync() =>
        [.. await _personRepository.GetAllAsync()];

    public List<Person> GetAllPersons() => GetAllPersonsAsync().Result;

    public async Task<List<Person>> GetPersonsVerifieesAsync() =>
        [.. await _personRepository.GetVerifiedAsync()];

    public List<Person> GetPersonsVerifiees() => GetPersonsVerifieesAsync().Result;

    public async Task<List<Alveole>> GetAllAlveolesAsync() =>
        [.. await _alveoleRepository.GetAllAsync()];

    public List<Alveole> GetAllAlveoles() => GetAllAlveolesAsync().Result;

    public async Task<List<Alveole>> GetAlveolesVerifieesAsync() =>
        [.. await _alveoleRepository.GetVerifiedAsync()];

    public List<Alveole> GetAlveolesVerifiees() => GetAlveolesVerifieesAsync().Result;

    public async Task<Person?> GetPersonByIdAsync(int id) =>
        await _personRepository.GetByIdAsync(id);

    public Person? GetPersonById(int id) => GetPersonByIdAsync(id).Result;

    public async Task<Person?> GetPersonByTokenAsync(string token) =>
        await _personRepository.GetByTokenAsync(token);

    public Person? GetPersonByToken(string token) => GetPersonByTokenAsync(token).Result;

    public async Task<Alveole?> GetAlveoleByTokenAsync(string token) =>
        await _alveoleRepository.GetByTokenAsync(token);

    public Alveole? GetAlveoleByToken(string token) => GetAlveoleByTokenAsync(token).Result;

    public async Task<bool> VerifierEmailPersonAsync(string token) =>
        await _personRepository.VerifyEmailAsync(token);

    public bool VerifierEmailPerson(string token) => VerifierEmailPersonAsync(token).Result;

    public async Task<bool> VerifierEmailAlveoleAsync(string token) =>
        await _alveoleRepository.VerifyEmailAsync(token);

    public bool VerifierEmailAlveole(string token) => VerifierEmailAlveoleAsync(token).Result;
}