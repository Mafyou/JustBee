using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JustBeeInfrastructure.Data;

public static class DataSeeder
{
    // Helper class for logger generic type
    private class DataSeederLogger { }

    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<JustBeeContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeederLogger>>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Departements if not exist
        if (!await context.Departements.AnyAsync())
        {
            logger.LogInformation("Seeding departements...");
            await SeedDepartementsAsync(context);
            logger.LogInformation("Departements seeded successfully");
        }

        // Seed Villes if not exist - now using external API for all French cities
        if (!await context.Villes.AnyAsync())
        {
            logger.LogInformation("Seeding all French cities from government API...");
            await SeedAllFrenchVillesAsync(context, scope.ServiceProvider, logger);
            logger.LogInformation("Cities seeded successfully");
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedDepartementsAsync(JustBeeContext context)
    {
        var departements = new List<Departement>
        {
            new() { Code = "01", Nom = "Ain", Region = "Auvergne-Rhône-Alpes", Latitude = 46.2, Longitude = 5.2 },
            new() { Code = "02", Nom = "Aisne", Region = "Hauts-de-France", Latitude = 49.4, Longitude = 3.4 },
            new() { Code = "03", Nom = "Allier", Region = "Auvergne-Rhône-Alpes", Latitude = 46.3, Longitude = 3.3 },
            new() { Code = "04", Nom = "Alpes-de-Haute-Provence", Region = "Provence-Alpes-Côte d'Azur", Latitude = 44.1, Longitude = 6.2 },
            new() { Code = "05", Nom = "Hautes-Alpes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 44.7, Longitude = 6.5 },
            new() { Code = "06", Nom = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.7, Longitude = 7.3 },
            new() { Code = "07", Nom = "Ardèche", Region = "Auvergne-Rhône-Alpes", Latitude = 44.7, Longitude = 4.6 },
            new() { Code = "08", Nom = "Ardennes", Region = "Grand Est", Latitude = 49.8, Longitude = 4.7 },
            new() { Code = "09", Nom = "Ariège", Region = "Occitanie", Latitude = 42.9, Longitude = 1.6 },
            new() { Code = "10", Nom = "Aube", Region = "Grand Est", Latitude = 48.3, Longitude = 4.1 },
            new() { Code = "11", Nom = "Aude", Region = "Occitanie", Latitude = 43.2, Longitude = 2.4 },
            new() { Code = "12", Nom = "Aveyron", Region = "Occitanie", Latitude = 44.4, Longitude = 2.6 },
            new() { Code = "13", Nom = "Bouches-du-Rhône", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5, Longitude = 5.4 },
            new() { Code = "14", Nom = "Calvados", Region = "Normandie", Latitude = 49.2, Longitude = -0.4 },
            new() { Code = "15", Nom = "Cantal", Region = "Auvergne-Rhône-Alpes", Latitude = 45.0, Longitude = 2.4 },
            new() { Code = "16", Nom = "Charente", Region = "Nouvelle-Aquitaine", Latitude = 45.7, Longitude = 0.2 },
            new() { Code = "17", Nom = "Charente-Maritime", Region = "Nouvelle-Aquitaine", Latitude = 45.7, Longitude = -0.8 },
            new() { Code = "18", Nom = "Cher", Region = "Centre-Val de Loire", Latitude = 47.1, Longitude = 2.4 },
            new() { Code = "19", Nom = "Corrèze", Region = "Nouvelle-Aquitaine", Latitude = 45.3, Longitude = 2.0 },
            new() { Code = "2A", Nom = "Corse-du-Sud", Region = "Corse", Latitude = 41.9, Longitude = 9.0 },
            new() { Code = "2B", Nom = "Haute-Corse", Region = "Corse", Latitude = 42.4, Longitude = 9.2 },
            new() { Code = "21", Nom = "Côte-d'Or", Region = "Bourgogne-Franche-Comté", Latitude = 47.3, Longitude = 4.8 },
            new() { Code = "22", Nom = "Côtes-d'Armor", Region = "Bretagne", Latitude = 48.4, Longitude = -2.8 },
            new() { Code = "23", Nom = "Creuse", Region = "Nouvelle-Aquitaine", Latitude = 46.0, Longitude = 2.0 },
            new() { Code = "24", Nom = "Dordogne", Region = "Nouvelle-Aquitaine", Latitude = 45.2, Longitude = 0.7 },
            new() { Code = "25", Nom = "Doubs", Region = "Bourgogne-Franche-Comté", Latitude = 47.2, Longitude = 6.1 },
            new() { Code = "26", Nom = "Drôme", Region = "Auvergne-Rhône-Alpes", Latitude = 44.7, Longitude = 5.1 },
            new() { Code = "27", Nom = "Eure", Region = "Normandie", Latitude = 49.0, Longitude = 1.2 },
            new() { Code = "28", Nom = "Eure-et-Loir", Region = "Centre-Val de Loire", Latitude = 48.4, Longitude = 1.5 },
            new() { Code = "29", Nom = "Finistère", Region = "Bretagne", Latitude = 48.2, Longitude = -4.1 },
            new() { Code = "30", Nom = "Gard", Region = "Occitanie", Latitude = 44.0, Longitude = 4.2 },
            new() { Code = "31", Nom = "Haute-Garonne", Region = "Occitanie", Latitude = 43.4, Longitude = 1.4 },
            new() { Code = "32", Nom = "Gers", Region = "Occitanie", Latitude = 43.7, Longitude = 0.6 },
            new() { Code = "33", Nom = "Gironde", Region = "Nouvelle-Aquitaine", Latitude = 44.8, Longitude = -0.6 },
            new() { Code = "34", Nom = "Hérault", Region = "Occitanie", Latitude = 43.6, Longitude = 3.9 },
            new() { Code = "35", Nom = "Ille-et-Vilaine", Region = "Bretagne", Latitude = 48.1, Longitude = -1.7 },
            new() { Code = "36", Nom = "Indre", Region = "Centre-Val de Loire", Latitude = 46.7, Longitude = 1.6 },
            new() { Code = "37", Nom = "Indre-et-Loire", Region = "Centre-Val de Loire", Latitude = 47.4, Longitude = 0.7 },
            new() { Code = "38", Nom = "Isère", Region = "Auvergne-Rhône-Alpes", Latitude = 45.4, Longitude = 5.6 },
            new() { Code = "39", Nom = "Jura", Region = "Bourgogne-Franche-Comté", Latitude = 46.7, Longitude = 5.8 },
            new() { Code = "40", Nom = "Landes", Region = "Nouvelle-Aquitaine", Latitude = 44.0, Longitude = -0.8 },
            new() { Code = "41", Nom = "Loir-et-Cher", Region = "Centre-Val de Loire", Latitude = 47.6, Longitude = 1.3 },
            new() { Code = "42", Nom = "Loire", Region = "Auvergne-Rhône-Alpes", Latitude = 45.4, Longitude = 4.4 },
            new() { Code = "43", Nom = "Haute-Loire", Region = "Auvergne-Rhône-Alpes", Latitude = 45.0, Longitude = 3.9 },
            new() { Code = "44", Nom = "Loire-Atlantique", Region = "Pays de la Loire", Latitude = 47.2, Longitude = -1.6 },
            new() { Code = "45", Nom = "Loiret", Region = "Centre-Val de Loire", Latitude = 47.9, Longitude = 2.1 },
            new() { Code = "46", Nom = "Lot", Region = "Occitanie", Latitude = 44.4, Longitude = 1.4 },
            new() { Code = "47", Nom = "Lot-et-Garonne", Region = "Nouvelle-Aquitaine", Latitude = 44.2, Longitude = 0.6 },
            new() { Code = "48", Nom = "Lozère", Region = "Occitanie", Latitude = 44.5, Longitude = 3.5 },
            new() { Code = "49", Nom = "Maine-et-Loire", Region = "Pays de la Loire", Latitude = 47.5, Longitude = -0.9 },
            new() { Code = "50", Nom = "Manche", Region = "Normandie", Latitude = 49.1, Longitude = -1.3 },
            new() { Code = "51", Nom = "Marne", Region = "Grand Est", Latitude = 49.0, Longitude = 4.0 },
            new() { Code = "52", Nom = "Haute-Marne", Region = "Grand Est", Latitude = 48.1, Longitude = 5.3 },
            new() { Code = "53", Nom = "Mayenne", Region = "Pays de la Loire", Latitude = 48.3, Longitude = -0.6 },
            new() { Code = "54", Nom = "Meurthe-et-Moselle", Region = "Grand Est", Latitude = 48.7, Longitude = 6.2 },
            new() { Code = "55", Nom = "Meuse", Region = "Grand Est", Latitude = 49.0, Longitude = 5.4 },
            new() { Code = "56", Nom = "Morbihan", Region = "Bretagne", Latitude = 47.7, Longitude = -2.7 },
            new() { Code = "57", Nom = "Moselle", Region = "Grand Est", Latitude = 49.1, Longitude = 6.7 },
            new() { Code = "58", Nom = "Nièvre", Region = "Bourgogne-Franche-Comté", Latitude = 47.0, Longitude = 3.5 },
            new() { Code = "59", Nom = "Nord", Region = "Hauts-de-France", Latitude = 50.5, Longitude = 3.2 },
            new() { Code = "60", Nom = "Oise", Region = "Hauts-de-France", Latitude = 49.4, Longitude = 2.8 },
            new() { Code = "61", Nom = "Orne", Region = "Normandie", Latitude = 48.6, Longitude = 0.1 },
            new() { Code = "62", Nom = "Pas-de-Calais", Region = "Hauts-de-France", Latitude = 50.4, Longitude = 2.6 },
            new() { Code = "63", Nom = "Puy-de-Dôme", Region = "Auvergne-Rhône-Alpes", Latitude = 45.8, Longitude = 3.1 },
            new() { Code = "64", Nom = "Pyrénées-Atlantiques", Region = "Nouvelle-Aquitaine", Latitude = 43.3, Longitude = -1.0 },
            new() { Code = "65", Nom = "Hautes-Pyrénées", Region = "Occitanie", Latitude = 43.2, Longitude = 0.1 },
            new() { Code = "66", Nom = "Pyrénées-Orientales", Region = "Occitanie", Latitude = 42.7, Longitude = 2.9 },
            new() { Code = "67", Nom = "Bas-Rhin", Region = "Grand Est", Latitude = 48.6, Longitude = 7.8 },
            new() { Code = "68", Nom = "Haut-Rhin", Region = "Grand Est", Latitude = 47.8, Longitude = 7.3 },
            new() { Code = "69", Nom = "Rhône", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7, Longitude = 4.8 },
            new() { Code = "70", Nom = "Haute-Saône", Region = "Bourgogne-Franche-Comté", Latitude = 47.6, Longitude = 6.2 },
            new() { Code = "71", Nom = "Saône-et-Loire", Region = "Bourgogne-Franche-Comté", Latitude = 46.7, Longitude = 4.3 },
            new() { Code = "72", Nom = "Sarthe", Region = "Pays de la Loire", Latitude = 48.0, Longitude = 0.2 },
            new() { Code = "73", Nom = "Savoie", Region = "Auvergne-Rhône-Alpes", Latitude = 45.6, Longitude = 6.4 },
            new() { Code = "74", Nom = "Haute-Savoie", Region = "Auvergne-Rhône-Alpes", Latitude = 46.1, Longitude = 6.3 },
            new() { Code = "75", Nom = "Paris", Region = "Île-de-France", Latitude = 48.9, Longitude = 2.3 },
            new() { Code = "76", Nom = "Seine-Maritime", Region = "Normandie", Latitude = 49.4, Longitude = 1.1 },
            new() { Code = "77", Nom = "Seine-et-Marne", Region = "Île-de-France", Latitude = 48.5, Longitude = 2.9 },
            new() { Code = "78", Nom = "Yvelines", Region = "Île-de-France", Latitude = 48.8, Longitude = 2.0 },
            new() { Code = "79", Nom = "Deux-Sèvres", Region = "Nouvelle-Aquitaine", Latitude = 46.3, Longitude = -0.5 },
            new() { Code = "80", Nom = "Somme", Region = "Hauts-de-France", Latitude = 49.9, Longitude = 2.3 },
            new() { Code = "81", Nom = "Tarn", Region = "Occitanie", Latitude = 43.9, Longitude = 2.1 },
            new() { Code = "82", Nom = "Tarn-et-Garonne", Region = "Occitanie", Latitude = 44.0, Longitude = 1.4 },
            new() { Code = "83", Nom = "Var", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5, Longitude = 6.5 },
            new() { Code = "84", Nom = "Vaucluse", Region = "Provence-Alpes-Côte d'Azur", Latitude = 44.0, Longitude = 5.1 },
            new() { Code = "85", Nom = "Vendée", Region = "Pays de la Loire", Latitude = 46.7, Longitude = -1.4 },
            new() { Code = "86", Nom = "Vienne", Region = "Nouvelle-Aquitaine", Latitude = 46.6, Longitude = 0.3 },
            new() { Code = "87", Nom = "Haute-Vienne", Region = "Nouvelle-Aquitaine", Latitude = 45.8, Longitude = 1.3 },
            new() { Code = "88", Nom = "Vosges", Region = "Grand Est", Latitude = 48.2, Longitude = 6.5 },
            new() { Code = "89", Nom = "Yonne", Region = "Bourgogne-Franche-Comté", Latitude = 47.8, Longitude = 3.6 },
            new() { Code = "90", Nom = "Territoire de Belfort", Region = "Bourgogne-Franche-Comté", Latitude = 47.6, Longitude = 6.9 },
            new() { Code = "91", Nom = "Essonne", Region = "Île-de-France", Latitude = 48.6, Longitude = 2.4 },
            new() { Code = "92", Nom = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.8, Longitude = 2.2 },
            new() { Code = "93", Nom = "Seine-Saint-Denis", Region = "Île-de-France", Latitude = 48.9, Longitude = 2.5 },
            new() { Code = "94", Nom = "Val-de-Marne", Region = "Île-de-France", Latitude = 48.8, Longitude = 2.5 },
            new() { Code = "95", Nom = "Val-d'Oise", Region = "Île-de-France", Latitude = 49.0, Longitude = 2.1 }
        };

        await context.Departements.AddRangeAsync(departements);
    }

    private static async Task SeedAllFrenchVillesAsync(JustBeeContext context, IServiceProvider serviceProvider, ILogger<DataSeederLogger> logger)
    {
        try
        {
            logger.LogInformation("Loading all French cities from government API...");
            var villes = await LoadVillesDirectlyFromApiAsync(serviceProvider, logger);

            if (villes.Count == 0)
            {
                logger.LogWarning("No cities loaded from API, falling back to basic cities");
                villes = GetFallbackVilles();
            }

            if (villes.Count > 0)
            {
                // Process cities in batches to avoid memory issues and SQL Server timeout
                const int batchSize = 1000;
                var totalBatches = (int)Math.Ceiling((double)villes.Count / batchSize);

                logger.LogInformation($"Processing {villes.Count} cities in {totalBatches} batches of {batchSize}");

                for (int i = 0; i < totalBatches; i++)
                {
                    var batch = villes.Skip(i * batchSize).Take(batchSize).ToList();
                    
                    // Ensure navigation collections are initialized to prevent EF issues
                    foreach (var ville in batch)
                    {
                        ville.Persons = [];
                        ville.Alveoles = [];
                    }

                    await context.Villes.AddRangeAsync(batch);
                    
                    // Save each batch to avoid keeping too much in memory and prevent SQL timeouts
                    await context.SaveChangesAsync();
                    
                    logger.LogInformation($"Processed batch {i + 1}/{totalBatches} ({batch.Count} cities)");
                    
                    // Clear change tracker to free memory
                    context.ChangeTracker.Clear();
                }

                logger.LogInformation($"Successfully seeded {villes.Count} French cities");
            }
            else
            {
                logger.LogError("No cities were loaded - this should not happen");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while seeding cities");
            
            // Fallback to basic cities if everything fails
            logger.LogInformation("Falling back to basic cities due to error");
            var fallbackVilles = GetFallbackVilles();
            
            foreach (var ville in fallbackVilles)
            {
                ville.Persons = [];
                ville.Alveoles = [];
            }
            
            await context.Villes.AddRangeAsync(fallbackVilles);
            logger.LogInformation($"Seeded {fallbackVilles.Count} fallback cities");
        }
    }

    private static async Task<List<Ville>> LoadVillesDirectlyFromApiAsync(IServiceProvider serviceProvider, ILogger<DataSeederLogger> logger)
    {
        try
        {
            // Create HttpClient directly since IHttpClientFactory might not be available
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(5); // Increase timeout for large API response

            logger.LogInformation("Making direct API call to load French cities...");
            
            var response = await httpClient.GetAsync("https://geo.api.gouv.fr/communes?fields=nom,code,codeDepartement,departement,codeRegion,region,centre&format=json");

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning($"API call failed with status: {response.StatusCode}");
                return [];
            }

            var json = await response.Content.ReadAsStringAsync();
            
            // Parse the JSON manually since we don't have access to serialization context here
            using var document = JsonDocument.Parse(json);
            var villes = new List<Ville>();

            foreach (var element in document.RootElement.EnumerateArray())
            {
                try
                {
                    var code = element.GetProperty("code").GetString();
                    var nom = element.GetProperty("nom").GetString();
                    var departementNom = element.GetProperty("departement").GetProperty("nom").GetString();
                    var regionNom = element.GetProperty("region").GetProperty("nom").GetString();
                    
                    if (element.TryGetProperty("centre", out var centreElement) && 
                        centreElement.TryGetProperty("coordinates", out var coordsElement) &&
                        coordsElement.GetArrayLength() == 2)
                    {
                        var longitude = coordsElement[0].GetDouble();
                        var latitude = coordsElement[1].GetDouble();

                        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(nom))
                        {
                            villes.Add(new Ville
                            {
                                Code = code,
                                Nom = nom,
                                Departement = departementNom ?? "",
                                Region = regionNom ?? "",
                                Latitude = latitude,
                                Longitude = longitude,
                                Persons = [],
                                Alveoles = []
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Skip malformed entries
                    logger.LogDebug($"Skipping malformed city entry: {ex.Message}");
                }
            }

            logger.LogInformation($"Loaded {villes.Count} cities from direct API call");
            return villes;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading cities directly from API");
            return [];
        }
    }

    private static List<Ville> GetFallbackVilles()
    {
        // Fallback to essential French cities if API fails
        return new List<Ville>
        {
            // Grandes métropoles with official INSEE codes
            new() { Code = "75056", Nom = "Paris", Departement = "Paris", Region = "Île-de-France", Latitude = 48.8566, Longitude = 2.3522, Persons = [], Alveoles = [] },
            new() { Code = "13055", Nom = "Marseille", Departement = "Bouches-du-Rhône", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.2965, Longitude = 5.3698, Persons = [], Alveoles = [] },
            new() { Code = "69123", Nom = "Lyon", Departement = "Rhône", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7640, Longitude = 4.8357, Persons = [], Alveoles = [] },
            new() { Code = "31555", Nom = "Toulouse", Departement = "Haute-Garonne", Region = "Occitanie", Latitude = 43.6047, Longitude = 1.4442, Persons = [], Alveoles = [] },
            new() { Code = "06088", Nom = "Nice", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.7102, Longitude = 7.2620, Persons = [], Alveoles = [] },
            new() { Code = "44109", Nom = "Nantes", Departement = "Loire-Atlantique", Region = "Pays de la Loire", Latitude = 47.2184, Longitude = -1.5536, Persons = [], Alveoles = [] },
            new() { Code = "34172", Nom = "Montpellier", Departement = "Hérault", Region = "Occitanie", Latitude = 43.6110, Longitude = 3.8767, Persons = [], Alveoles = [] },
            new() { Code = "67482", Nom = "Strasbourg", Departement = "Bas-Rhin", Region = "Grand Est", Latitude = 48.5734, Longitude = 7.7521, Persons = [], Alveoles = [] },
            new() { Code = "33063", Nom = "Bordeaux", Departement = "Gironde", Region = "Nouvelle-Aquitaine", Latitude = 44.8378, Longitude = -0.5792, Persons = [], Alveoles = [] },
            new() { Code = "59350", Nom = "Lille", Departement = "Nord", Region = "Hauts-de-France", Latitude = 50.6292, Longitude = 3.0573, Persons = [], Alveoles = [] },
            // Add a few more important cities including Carcès
            new() { Code = "35238", Nom = "Rennes", Departement = "Ille-et-Vilaine", Region = "Bretagne", Latitude = 48.1173, Longitude = -1.6778, Persons = [], Alveoles = [] },
            new() { Code = "51454", Nom = "Reims", Departement = "Marne", Region = "Grand Est", Latitude = 49.2583, Longitude = 4.0317, Persons = [], Alveoles = [] },
            new() { Code = "42218", Nom = "Saint-Étienne", Departement = "Loire", Region = "Auvergne-Rhône-Alpes", Latitude = 45.4397, Longitude = 4.3872, Persons = [], Alveoles = [] },
            new() { Code = "83137", Nom = "Toulon", Departement = "Var", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.1242, Longitude = 5.9280, Persons = [], Alveoles = [] },
            new() { Code = "38185", Nom = "Grenoble", Departement = "Isère", Region = "Auvergne-Rhône-Alpes", Latitude = 45.1885, Longitude = 5.7245, Persons = [], Alveoles = [] },
            new() { Code = "83049", Nom = "Carcès", Departement = "Var", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.4725, Longitude = 6.1836, Persons = [], Alveoles = [] }
        };
    }
}