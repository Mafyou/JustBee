using Microsoft.Extensions.Caching.Hybrid;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace JustBeeWeb.Services;

public class VilleDataService(HttpClient httpClient, ILogger<VilleDataService> logger, HybridCache cache)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<VilleDataService> _logger = logger;
    private readonly HybridCache _cache = cache;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<List<Ville>> GetAllVillesFranceAsync()
    {
        const string cacheKey = "AllVillesFrance";

        return await _cache.GetOrCreateAsync(
            cacheKey,
            async cancellationToken =>
            {
                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    _logger.LogInformation("Chargement des donn�es des villes fran�aises...");

                    // Charger depuis l'API officielle fran�aise
                    var villes = await LoadVillesFromApiAsync();

                    // Fallback : charger les villes de base si l'API �choue
                    if (villes.Count == 0)
                    {
                        _logger.LogWarning("Impossible de charger depuis l'API, utilisation des donn�es de base");
                        villes = GetVillesDeBase();
                    }

                    var result = villes.OrderBy(v => v.Nom).ToList();
                    _logger.LogInformation($"Chargement termin� : {result.Count} villes disponibles");

                    return result;
                }
                finally
                {
                    _semaphore.Release();
                }
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(6), // Cache plus long pour les villes
                LocalCacheExpiration = TimeSpan.FromHours(2)
            }
        );
    }

    public async Task<List<Ville>> SearchVillesAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetVillesPopulairesAsync();

        var terme = NormalizeString(searchTerm.ToLowerInvariant().Trim());

        // Cache bas� sur le terme de recherche normalis� pour �viter les recherches r�p�t�es
        var cacheKey = $"VilleSearch_{terme}";

        return await _cache.GetOrCreateAsync(
            cacheKey,
            async cancellationToken =>
            {
                var toutes = await GetAllVillesFranceAsync();

                var results = toutes
                    .Where(v =>
                        // Recherche avec normalisation des accents
                        NormalizeString(v.Nom.ToLowerInvariant()).Contains(terme) ||
                        NormalizeString(v.Code.ToLowerInvariant()).Contains(terme) ||
                        NormalizeString(v.Departement.ToLowerInvariant()).Contains(terme) ||
                        NormalizeString(v.Region.ToLowerInvariant()).Contains(terme) ||
                        // Recherche en d�but de mot pour une meilleure pertinence (avec normalisation)
                        NormalizeString(v.Nom.ToLowerInvariant()).StartsWith(terme) ||
                        NormalizeString(v.Departement.ToLowerInvariant()).StartsWith(terme) ||
                        NormalizeString(v.Region.ToLowerInvariant()).StartsWith(terme))
                    .OrderBy(v => NormalizeString(v.Nom.ToLowerInvariant()).StartsWith(terme) ? 0 : 1) // Prioriser les r�sultats qui commencent par le terme
                    .ThenBy(v => v.Nom)
                    .Take(50) // Limiter drastiquement les r�sultats pour de meilleures performances
                    .ToList();

                return results;
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(15), // Cache court pour les recherches
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            }
        );
    }

    /// <summary>
    /// Normalise une cha�ne en supprimant les accents et caract�res diacritiques pour une recherche insensible aux accents
    /// </summary>
    private static string NormalizeString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Normaliser vers la forme canonique d�compos�e (NFD)
        var normalizedString = input.Normalize(NormalizationForm.FormD);

        // Supprimer tous les caract�res diacritiques (accents)
        var stringBuilder = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        // Retourner la forme normalis�e (NFC)
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public async Task<List<Ville>> GetVillesPopulairesAsync()
    {
        const string cacheKey = "VillesPopulaires";

        return await _cache.GetOrCreateAsync(
            cacheKey,
            async cancellationToken =>
            {
                // Retourner directement les villes populaires sans charger toutes les villes
                return GetVillesDeBase().Take(30).ToList();
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(12),
                LocalCacheExpiration = TimeSpan.FromHours(6)
            }
        );
    }

    public async Task<List<Ville>> GetVillesWithLimitAsync(int limit = 100, int skip = 0)
    {
        var toutes = await GetAllVillesFranceAsync();

        return toutes
            .Skip(skip)
            .Take(Math.Min(limit, 200)) // Limiter � 200 max
            .ToList();
    }

    public async Task<int> GetTotalVillesCountAsync()
    {
        var toutes = await GetAllVillesFranceAsync();
        return toutes.Count;
    }

    private async Task<List<Ville>> LoadVillesFromApiAsync()
    {
        try
        {
            // Limiter la requ�te API aux champs n�cessaires et populations > 1000 pour r�duire la charge
            var apiUrl = "https://geo.api.gouv.fr/communes?fields=nom,code,codeDepartement,departement,codeRegion,region,centre,population&format=json&population>=1000";

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)); // Timeout de 10s
            var response = await _httpClient.GetAsync(apiUrl, cts.Token);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"�chec de l'appel API : {response.StatusCode}");
                return [];
            }

            var json = await response.Content.ReadAsStringAsync(cts.Token);
            // Use the source generation context for deserialization
            var communesApi = JsonSerializer.Deserialize<CommuneApiResponse[]>(json, MapBeeSerializationContext.Default.CommuneApiResponseArray);

            if (communesApi == null || communesApi.Length == 0)
            {
                _logger.LogWarning("Aucune donn�e re�ue de l'API");
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

            _logger.LogInformation($"Charg� {villes.Count} villes depuis l'API gouvernementale");
            return villes;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Timeout lors du chargement des villes depuis l'API");
            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du chargement des villes depuis l'API");
            return [];
        }
    }

    private List<Ville> GetVillesDeBase()
    {
        // Retourner les villes de base d�finies dans VilleService
        // Cette m�thode sert de fallback - Ajouter Carc�s pour les tests
        return
        [
            // Grandes m�tropoles
            new() { Code = "75056", Nom = "Paris", Departement = "Paris", Region = "�le-de-France", Latitude = 48.8566, Longitude = 2.3522 },
            new() { Code = "13055", Nom = "Marseille", Departement = "Bouches-du-Rh�ne", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.2965, Longitude = 5.3698 },
            new() { Code = "69123", Nom = "Lyon", Departement = "Rh�ne", Region = "Auvergne-Rh�ne-Alpes", Latitude = 45.7640, Longitude = 4.8357 },
            new() { Code = "31555", Nom = "Toulouse", Departement = "Haute-Garonne", Region = "Occitanie", Latitude = 43.6047, Longitude = 1.4442 },
            new() { Code = "06088", Nom = "Nice", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.7102, Longitude = 7.2620 },
            new() { Code = "44109", Nom = "Nantes", Departement = "Loire-Atlantique", Region = "Pays de la Loire", Latitude = 47.2184, Longitude = -1.5536 },
            new() { Code = "34172", Nom = "Montpellier", Departement = "H�rault", Region = "Occitanie", Latitude = 43.6110, Longitude = 3.8767 },
            new() { Code = "67482", Nom = "Strasbourg", Departement = "Bas-Rhin", Region = "Grand Est", Latitude = 48.5734, Longitude = 7.7521 },
            new() { Code = "33063", Nom = "Bordeaux", Departement = "Gironde", Region = "Nouvelle-Aquitaine", Latitude = 44.8378, Longitude = -0.5792 },
            new() { Code = "59350", Nom = "Lille", Departement = "Nord", Region = "Hauts-de-France", Latitude = 50.6292, Longitude = 3.0573 },
            
            // Autres grandes villes
            new() { Code = "35238", Nom = "Rennes", Departement = "Ille-et-Vilaine", Region = "Bretagne", Latitude = 48.1173, Longitude = -1.6778 },
            new() { Code = "51454", Nom = "Reims", Departement = "Marne", Region = "Grand Est", Latitude = 49.2583, Longitude = 4.0317 },
            new() { Code = "42218", Nom = "Saint-�tienne", Departement = "Loire", Region = "Auvergne-Rh�ne-Alpes", Latitude = 45.4397, Longitude = 4.3872 },
            new() { Code = "83137", Nom = "Toulon", Departement = "Var", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.1242, Longitude = 5.9280 },
            new() { Code = "38185", Nom = "Grenoble", Departement = "Is�re", Region = "Auvergne-Rh�ne-Alpes", Latitude = 45.1885, Longitude = 5.7245 },
            new() { Code = "21231", Nom = "Dijon", Departement = "C�te-d'Or", Region = "Bourgogne-Franche-Comt�", Latitude = 47.3220, Longitude = 5.0415 },
            new() { Code = "49007", Nom = "Angers", Departement = "Maine-et-Loire", Region = "Pays de la Loire", Latitude = 47.4784, Longitude = -0.5632 },
            new() { Code = "30189", Nom = "N�mes", Departement = "Gard", Region = "Occitanie", Latitude = 43.8367, Longitude = 4.3601 },
            new() { Code = "69266", Nom = "Villeurbanne", Departement = "Rh�ne", Region = "Auvergne-Rh�ne-Alpes", Latitude = 45.7667, Longitude = 4.8833 },
            new() { Code = "72181", Nom = "Le Mans", Departement = "Sarthe", Region = "Pays de la Loire", Latitude = 48.0061, Longitude = 0.1996 },
            new() { Code = "13001", Nom = "Aix-en-Provence", Departement = "Bouches-du-Rh�ne", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.5297, Longitude = 5.4474 },
            new() { Code = "29019", Nom = "Brest", Departement = "Finist�re", Region = "Bretagne", Latitude = 48.3904, Longitude = -4.4861 },
            new() { Code = "37261", Nom = "Tours", Departement = "Indre-et-Loire", Region = "Centre-Val de Loire", Latitude = 47.3941, Longitude = 0.6848 },
            new() { Code = "80021", Nom = "Amiens", Departement = "Somme", Region = "Hauts-de-France", Latitude = 49.8941, Longitude = 2.2958 },
            new() { Code = "87085", Nom = "Limoges", Departement = "Haute-Vienne", Region = "Nouvelle-Aquitaine", Latitude = 45.8336, Longitude = 1.2611 },
            new() { Code = "63113", Nom = "Clermont-Ferrand", Departement = "Puy-de-D�me", Region = "Auvergne-Rh�ne-Alpes", Latitude = 45.7797, Longitude = 3.0863 },
            new() { Code = "25056", Nom = "Besan�on", Departement = "Doubs", Region = "Bourgogne-Franche-Comt�", Latitude = 47.2378, Longitude = 6.0241 },
            new() { Code = "45234", Nom = "Orl�ans", Departement = "Loiret", Region = "Centre-Val de Loire", Latitude = 47.9029, Longitude = 1.9093 },
            new() { Code = "68224", Nom = "Mulhouse", Departement = "Haut-Rhin", Region = "Grand Est", Latitude = 47.7508, Longitude = 7.3359 },
            new() { Code = "76540", Nom = "Rouen", Departement = "Seine-Maritime", Region = "Normandie", Latitude = 49.4431, Longitude = 1.0993 },
            new() { Code = "54395", Nom = "Nancy", Departement = "Meurthe-et-Moselle", Region = "Grand Est", Latitude = 48.6921, Longitude = 6.1844 },
            new() { Code = "14118", Nom = "Caen", Departement = "Calvados", Region = "Normandie", Latitude = 49.1858, Longitude = -0.3708 },
            new() { Code = "84007", Nom = "Avignon", Departement = "Vaucluse", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.9493, Longitude = 4.8055 },
            new() { Code = "86194", Nom = "Poitiers", Departement = "Vienne", Region = "Nouvelle-Aquitaine", Latitude = 46.5802, Longitude = 0.3404 },
            new() { Code = "17300", Nom = "La Rochelle", Departement = "Charente-Maritime", Region = "Nouvelle-Aquitaine", Latitude = 46.1603, Longitude = -1.1511 },
            new() { Code = "64445", Nom = "Pau", Departement = "Pyr�n�es-Atlantiques", Region = "Nouvelle-Aquitaine", Latitude = 43.2951, Longitude = -0.3712 },
            new() { Code = "06004", Nom = "Antibes", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.5808, Longitude = 7.1251 },
            new() { Code = "06029", Nom = "Cannes", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.5528, Longitude = 7.0174 },
            new() { Code = "66136", Nom = "Perpignan", Departement = "Pyr�n�es-Orientales", Region = "Occitanie", Latitude = 42.6976, Longitude = 2.8954 },
            new() { Code = "78646", Nom = "Versailles", Departement = "Yvelines", Region = "�le-de-France", Latitude = 48.8014, Longitude = 2.1301 },
            new() { Code = "97209", Nom = "Fort-de-France", Departement = "Martinique", Region = "Martinique", Latitude = 14.6037, Longitude = -61.0594 },
            // Ajouter Carc�s pour les tests d'accents
            new() { Code = "83027", Nom = "Carc�s", Departement = "Var", Region = "Provence-Alpes-C�te d'Azur", Latitude = 43.4761, Longitude = 6.1856 }
        ];
    }
}