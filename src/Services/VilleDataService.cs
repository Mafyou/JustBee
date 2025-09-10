using JustBeeWeb.Models;
using System.Text.Json;

namespace JustBeeWeb.Services;

public class VilleDataService(HttpClient httpClient, ILogger<VilleDataService> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<VilleDataService> _logger = logger;
    private List<Ville>? _villesCache;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<List<Ville>> GetAllVillesFranceAsync()
    {
        if (_villesCache != null)
            return _villesCache;

        await _semaphore.WaitAsync();
        try
        {
            if (_villesCache != null)
                return _villesCache;

            _logger.LogInformation("Chargement des données des villes françaises...");
            
            // Charger depuis l'API officielle française
            var villes = await LoadVillesFromApiAsync();
            
            // Fallback : charger les villes de base si l'API échoue
            if (villes.Count == 0)
            {
                _logger.LogWarning("Impossible de charger depuis l'API, utilisation des données de base");
                villes = GetVillesDeBase();
            }

            _villesCache = villes.OrderBy(v => v.Nom).ToList();
            _logger.LogInformation($"Chargement terminé : {_villesCache.Count} villes disponibles");
            
            return _villesCache;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<Ville>> SearchVillesAsync(string searchTerm)
    {
        var toutes = await GetAllVillesFranceAsync();
        
        if (string.IsNullOrWhiteSpace(searchTerm))
            return toutes.Take(50).ToList(); // Limiter pour les performances

        var terme = searchTerm.ToLowerInvariant();
        
        return toutes
            .Where(v => 
                v.Nom.ToLowerInvariant().Contains(terme) ||
                v.Code.ToLowerInvariant().Contains(terme) ||
                v.Departement.ToLowerInvariant().Contains(terme) ||
                v.Region.ToLowerInvariant().Contains(terme))
            .Take(100) // Limiter les résultats pour les performances
            .ToList();
    }

    private async Task<List<Ville>> LoadVillesFromApiAsync()
    {
        try
        {
            // Utiliser l'API officielle geo.api.gouv.fr
            var response = await _httpClient.GetAsync("https://geo.api.gouv.fr/communes?fields=nom,code,codeDepartement,departement,codeRegion,region,centre&format=json");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Échec de l'appel API : {response.StatusCode}");
                return [];
            }

            var json = await response.Content.ReadAsStringAsync();
            var communesApi = JsonSerializer.Deserialize<CommuneApiResponse[]>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (communesApi == null || communesApi.Length == 0)
            {
                _logger.LogWarning("Aucune donnée reçue de l'API");
                return [];
            }

            var villes = communesApi
                .Where(c => c.Centre?.Coordinates != null && c.Centre.Coordinates.Length == 2)
                .Select(c => new Ville
                {
                    Code = c.Code ?? "",
                    Nom = c.Nom ?? "",
                    Departement = c.Departement?.Nom ?? "",
                    Region = c.Region?.Nom ?? "",
                    Latitude = c.Centre?.Coordinates[1] ?? 0,
                    Longitude = c.Centre?.Coordinates[0] ?? 0
                })
                .Where(v => !string.IsNullOrEmpty(v.Code) && !string.IsNullOrEmpty(v.Nom))
                .ToList();

            _logger.LogInformation($"Chargé {villes.Count} villes depuis l'API gouvernementale");
            return villes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du chargement des villes depuis l'API");
            return [];
        }
    }

    private List<Ville> GetVillesDeBase()
    {
        // Retourner les villes de base définies dans VilleService
        // Cette méthode sert de fallback
        return
        [
            // Grandes métropoles
            new() { Code = "75056", Nom = "Paris", Departement = "Paris", Region = "Île-de-France", Latitude = 48.8566, Longitude = 2.3522 },
            new() { Code = "13055", Nom = "Marseille", Departement = "Bouches-du-Rhône", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.2965, Longitude = 5.3698 },
            new() { Code = "69123", Nom = "Lyon", Departement = "Rhône", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7640, Longitude = 4.8357 },
            new() { Code = "31555", Nom = "Toulouse", Departement = "Haute-Garonne", Region = "Occitanie", Latitude = 43.6047, Longitude = 1.4442 },
            new() { Code = "06088", Nom = "Nice", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.7102, Longitude = 7.2620 },
            new() { Code = "44109", Nom = "Nantes", Departement = "Loire-Atlantique", Region = "Pays de la Loire", Latitude = 47.2184, Longitude = -1.5536 },
            new() { Code = "34172", Nom = "Montpellier", Departement = "Hérault", Region = "Occitanie", Latitude = 43.6110, Longitude = 3.8767 },
            new() { Code = "67482", Nom = "Strasbourg", Departement = "Bas-Rhin", Region = "Grand Est", Latitude = 48.5734, Longitude = 7.7521 },
            new() { Code = "33063", Nom = "Bordeaux", Departement = "Gironde", Region = "Nouvelle-Aquitaine", Latitude = 44.8378, Longitude = -0.5792 },
            new() { Code = "59350", Nom = "Lille", Departement = "Nord", Region = "Hauts-de-France", Latitude = 50.6292, Longitude = 3.0573 },
            
            // Autres grandes villes
            new() { Code = "35238", Nom = "Rennes", Departement = "Ille-et-Vilaine", Region = "Bretagne", Latitude = 48.1173, Longitude = -1.6778 },
            new() { Code = "51454", Nom = "Reims", Departement = "Marne", Region = "Grand Est", Latitude = 49.2583, Longitude = 4.0317 },
            new() { Code = "42218", Nom = "Saint-Étienne", Departement = "Loire", Region = "Auvergne-Rhône-Alpes", Latitude = 45.4397, Longitude = 4.3872 },
            new() { Code = "83137", Nom = "Toulon", Departement = "Var", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.1242, Longitude = 5.9280 },
            new() { Code = "38185", Nom = "Grenoble", Departement = "Isère", Region = "Auvergne-Rhône-Alpes", Latitude = 45.1885, Longitude = 5.7245 },
            new() { Code = "21231", Nom = "Dijon", Departement = "Côte-d'Or", Region = "Bourgogne-Franche-Comté", Latitude = 47.3220, Longitude = 5.0415 },
            new() { Code = "49007", Nom = "Angers", Departement = "Maine-et-Loire", Region = "Pays de la Loire", Latitude = 47.4784, Longitude = -0.5632 },
            new() { Code = "30189", Nom = "Nîmes", Departement = "Gard", Region = "Occitanie", Latitude = 43.8367, Longitude = 4.3601 },
            new() { Code = "69266", Nom = "Villeurbanne", Departement = "Rhône", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7667, Longitude = 4.8833 },
            new() { Code = "72181", Nom = "Le Mans", Departement = "Sarthe", Region = "Pays de la Loire", Latitude = 48.0061, Longitude = 0.1996 },
            new() { Code = "13001", Nom = "Aix-en-Provence", Departement = "Bouches-du-Rhône", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5297, Longitude = 5.4474 },
            new() { Code = "29019", Nom = "Brest", Departement = "Finistère", Region = "Bretagne", Latitude = 48.3904, Longitude = -4.4861 },
            new() { Code = "37261", Nom = "Tours", Departement = "Indre-et-Loire", Region = "Centre-Val de Loire", Latitude = 47.3941, Longitude = 0.6848 },
            new() { Code = "80021", Nom = "Amiens", Departement = "Somme", Region = "Hauts-de-France", Latitude = 49.8941, Longitude = 2.2958 },
            new() { Code = "87085", Nom = "Limoges", Departement = "Haute-Vienne", Region = "Nouvelle-Aquitaine", Latitude = 45.8336, Longitude = 1.2611 },
            new() { Code = "63113", Nom = "Clermont-Ferrand", Departement = "Puy-de-Dôme", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7797, Longitude = 3.0863 },
            new() { Code = "25056", Nom = "Besançon", Departement = "Doubs", Region = "Bourgogne-Franche-Comté", Latitude = 47.2378, Longitude = 6.0241 },
            new() { Code = "45234", Nom = "Orléans", Departement = "Loiret", Region = "Centre-Val de Loire", Latitude = 47.9029, Longitude = 1.9093 },
            new() { Code = "68224", Nom = "Mulhouse", Departement = "Haut-Rhin", Region = "Grand Est", Latitude = 47.7508, Longitude = 7.3359 },
            new() { Code = "76540", Nom = "Rouen", Departement = "Seine-Maritime", Region = "Normandie", Latitude = 49.4431, Longitude = 1.0993 },
            new() { Code = "54395", Nom = "Nancy", Departement = "Meurthe-et-Moselle", Region = "Grand Est", Latitude = 48.6921, Longitude = 6.1844 },
            new() { Code = "14118", Nom = "Caen", Departement = "Calvados", Region = "Normandie", Latitude = 49.1858, Longitude = -0.3708 },
            new() { Code = "84007", Nom = "Avignon", Departement = "Vaucluse", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.9493, Longitude = 4.8055 },
            new() { Code = "86194", Nom = "Poitiers", Departement = "Vienne", Region = "Nouvelle-Aquitaine", Latitude = 46.5802, Longitude = 0.3404 },
            new() { Code = "17300", Nom = "La Rochelle", Departement = "Charente-Maritime", Region = "Nouvelle-Aquitaine", Latitude = 46.1603, Longitude = -1.1511 },
            new() { Code = "64445", Nom = "Pau", Departement = "Pyrénées-Atlantiques", Region = "Nouvelle-Aquitaine", Latitude = 43.2951, Longitude = -0.3712 },
            new() { Code = "06004", Nom = "Antibes", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5808, Longitude = 7.1251 },
            new() { Code = "06029", Nom = "Cannes", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5528, Longitude = 7.0174 },
            new() { Code = "66136", Nom = "Perpignan", Departement = "Pyrénées-Orientales", Region = "Occitanie", Latitude = 42.6976, Longitude = 2.8954 },
            new() { Code = "78646", Nom = "Versailles", Departement = "Yvelines", Region = "Île-de-France", Latitude = 48.8014, Longitude = 2.1301 },
            new() { Code = "97209", Nom = "Fort-de-France", Departement = "Martinique", Region = "Martinique", Latitude = 14.6037, Longitude = -61.0594 }
        ];
    }
}

// Classes pour la désérialisation de l'API
public class CommuneApiResponse
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
    public DepartementApi? Departement { get; set; }
    public RegionApi? Region { get; set; }
    public CentreApi? Centre { get; set; }
}

public class DepartementApi
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
}

public class RegionApi
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
}

public class CentreApi
{
    public string? Type { get; set; }
    public double[]? Coordinates { get; set; }
}