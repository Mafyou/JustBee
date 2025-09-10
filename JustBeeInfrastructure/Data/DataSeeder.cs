using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JustBeeInfrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<JustBeeContext>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Departements if not exist
        if (!await context.Departements.AnyAsync())
        {
            await SeedDepartementsAsync(context);
        }

        // Seed Villes if not exist
        if (!await context.Villes.AnyAsync())
        {
            await SeedVillesAsync(context);
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

    private static async Task SeedVillesAsync(JustBeeContext context)
    {
        var villes = new List<Ville>
        {
            // Grandes métropoles
            new() { Code = "PARIS", Nom = "Paris", Departement = "Paris", Region = "Île-de-France", Latitude = 48.8566, Longitude = 2.3522 },
            new() { Code = "MARSEILLE", Nom = "Marseille", Departement = "Bouches-du-Rhône", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.2965, Longitude = 5.3698 },
            new() { Code = "LYON", Nom = "Lyon", Departement = "Rhône", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7640, Longitude = 4.8357 },
            new() { Code = "TOULOUSE", Nom = "Toulouse", Departement = "Haute-Garonne", Region = "Occitanie", Latitude = 43.6047, Longitude = 1.4442 },
            new() { Code = "NICE", Nom = "Nice", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.7102, Longitude = 7.2620 },
            new() { Code = "NANTES", Nom = "Nantes", Departement = "Loire-Atlantique", Region = "Pays de la Loire", Latitude = 47.2184, Longitude = -1.5536 },
            new() { Code = "MONTPELLIER", Nom = "Montpellier", Departement = "Hérault", Region = "Occitanie", Latitude = 43.6110, Longitude = 3.8767 },
            new() { Code = "STRASBOURG", Nom = "Strasbourg", Departement = "Bas-Rhin", Region = "Grand Est", Latitude = 48.5734, Longitude = 7.7521 },
            new() { Code = "BORDEAUX", Nom = "Bordeaux", Departement = "Gironde", Region = "Nouvelle-Aquitaine", Latitude = 44.8378, Longitude = -0.5792 },
            new() { Code = "LILLE", Nom = "Lille", Departement = "Nord", Region = "Hauts-de-France", Latitude = 50.6292, Longitude = 3.0573 },
            
            // Grandes villes régionales
            new() { Code = "RENNES", Nom = "Rennes", Departement = "Ille-et-Vilaine", Region = "Bretagne", Latitude = 48.1173, Longitude = -1.6778 },
            new() { Code = "REIMS", Nom = "Reims", Departement = "Marne", Region = "Grand Est", Latitude = 49.2583, Longitude = 4.0317 },
            new() { Code = "SAINTETIENNE", Nom = "Saint-Étienne", Departement = "Loire", Region = "Auvergne-Rhône-Alpes", Latitude = 45.4397, Longitude = 4.3872 },
            new() { Code = "TOULON", Nom = "Toulon", Departement = "Var", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.1242, Longitude = 5.9280 },
            new() { Code = "GRENOBLE", Nom = "Grenoble", Departement = "Isère", Region = "Auvergne-Rhône-Alpes", Latitude = 45.1885, Longitude = 5.7245 },
            new() { Code = "DIJON", Nom = "Dijon", Departement = "Côte-d'Or", Region = "Bourgogne-Franche-Comté", Latitude = 47.3220, Longitude = 5.0415 },
            new() { Code = "ANGERS", Nom = "Angers", Departement = "Maine-et-Loire", Region = "Pays de la Loire", Latitude = 47.4784, Longitude = -0.5632 },
            new() { Code = "NIMES", Nom = "Nîmes", Departement = "Gard", Region = "Occitanie", Latitude = 43.8367, Longitude = 4.3601 },
            new() { Code = "VILLEURBANNE", Nom = "Villeurbanne", Departement = "Rhône", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7667, Longitude = 4.8833 },
            new() { Code = "LEMANS", Nom = "Le Mans", Departement = "Sarthe", Region = "Pays de la Loire", Latitude = 48.0061, Longitude = 0.1996 },
            
            // Villes moyennes représentatives
            new() { Code = "AIXENPROVENCE", Nom = "Aix-en-Provence", Departement = "Bouches-du-Rhône", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5297, Longitude = 5.4474 },
            new() { Code = "BREST", Nom = "Brest", Departement = "Finistère", Region = "Bretagne", Latitude = 48.3904, Longitude = -4.4861 },
            new() { Code = "TOURS", Nom = "Tours", Departement = "Indre-et-Loire", Region = "Centre-Val de Loire", Latitude = 47.3941, Longitude = 0.6848 },
            new() { Code = "AMIENS", Nom = "Amiens", Departement = "Somme", Region = "Hauts-de-France", Latitude = 49.8941, Longitude = 2.2958 },
            new() { Code = "LIMOGES", Nom = "Limoges", Departement = "Haute-Vienne", Region = "Nouvelle-Aquitaine", Latitude = 45.8336, Longitude = 1.2611 },
            new() { Code = "CLERMONT", Nom = "Clermont-Ferrand", Departement = "Puy-de-Dôme", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7797, Longitude = 3.0863 },
            new() { Code = "BESANCON", Nom = "Besançon", Departement = "Doubs", Region = "Bourgogne-Franche-Comté", Latitude = 47.2378, Longitude = 6.0241 },
            new() { Code = "ORLEANS", Nom = "Orléans", Departement = "Loiret", Region = "Centre-Val de Loire", Latitude = 47.9029, Longitude = 1.9093 },
            new() { Code = "MULHOUSE", Nom = "Mulhouse", Departement = "Haut-Rhin", Region = "Grand Est", Latitude = 47.7508, Longitude = 7.3359 },
            new() { Code = "ROUEN", Nom = "Rouen", Departement = "Seine-Maritime", Region = "Normandie", Latitude = 49.4431, Longitude = 1.0993 },
            new() { Code = "NANCY", Nom = "Nancy", Departement = "Meurthe-et-Moselle", Region = "Grand Est", Latitude = 48.6921, Longitude = 6.1844 },
            new() { Code = "ARGENTEUIL", Nom = "Argenteuil", Departement = "Val-d'Oise", Region = "Île-de-France", Latitude = 48.9474, Longitude = 2.2476 },
            new() { Code = "MONTREUIL", Nom = "Montreuil", Departement = "Seine-Saint-Denis", Region = "Île-de-France", Latitude = 48.8634, Longitude = 2.4450 },
            new() { Code = "CAEN", Nom = "Caen", Departement = "Calvados", Region = "Normandie", Latitude = 49.1858, Longitude = -0.3708 },
            new() { Code = "TOURCOING", Nom = "Tourcoing", Departement = "Nord", Region = "Hauts-de-France", Latitude = 50.7236, Longitude = 3.1614 },
            new() { Code = "ROUBAIX", Nom = "Roubaix", Departement = "Nord", Region = "Hauts-de-France", Latitude = 50.6942, Longitude = 3.1746 },
            new() { Code = "NANTERRE", Nom = "Nanterre", Departement = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.8915, Longitude = 2.2066 },
            new() { Code = "AVIGNON", Nom = "Avignon", Departement = "Vaucluse", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.9493, Longitude = 4.8055 },
            new() { Code = "CRETEIL", Nom = "Créteil", Departement = "Val-de-Marne", Region = "Île-de-France", Latitude = 48.7903, Longitude = 2.4555 },
            new() { Code = "POITIERS", Nom = "Poitiers", Departement = "Vienne", Region = "Nouvelle-Aquitaine", Latitude = 46.5802, Longitude = 0.3404 },
            new() { Code = "DUNKERQUE", Nom = "Dunkerque", Departement = "Nord", Region = "Hauts-de-France", Latitude = 51.0342, Longitude = 2.3770 },
            new() { Code = "ASNIERESSURSEINE", Nom = "Asnières-sur-Seine", Departement = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.9152, Longitude = 2.2874 },
            new() { Code = "BOULOGNE", Nom = "Boulogne-Billancourt", Departement = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.8356, Longitude = 2.2397 },
            new() { Code = "PERPIGNAN", Nom = "Perpignan", Departement = "Pyrénées-Orientales", Region = "Occitanie", Latitude = 42.6976, Longitude = 2.8954 },
            new() { Code = "VERSAILLES", Nom = "Versailles", Departement = "Yvelines", Region = "Île-de-France", Latitude = 48.8014, Longitude = 2.1301 },
            new() { Code = "COLOMBES", Nom = "Colombes", Departement = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.9226, Longitude = 2.2572 },
            new() { Code = "FORTDEFRANCE", Nom = "Fort-de-France", Departement = "Martinique", Region = "Martinique", Latitude = 14.6037, Longitude = -61.0594 },
            new() { Code = "AULNAY", Nom = "Aulnay-sous-Bois", Departement = "Seine-Saint-Denis", Region = "Île-de-France", Latitude = 48.9349, Longitude = 2.4944 },
            new() { Code = "RUEILMALMAISON", Nom = "Rueil-Malmaison", Departement = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.8772, Longitude = 2.1760 },
            new() { Code = "ROCHELLE", Nom = "La Rochelle", Departement = "Charente-Maritime", Region = "Nouvelle-Aquitaine", Latitude = 46.1603, Longitude = -1.1511 },
            new() { Code = "PAU", Nom = "Pau", Departement = "Pyrénées-Atlantiques", Region = "Nouvelle-Aquitaine", Latitude = 43.2951, Longitude = -0.3712 },
            new() { Code = "AUBERVILLIERS", Nom = "Aubervilliers", Departement = "Seine-Saint-Denis", Region = "Île-de-France", Latitude = 48.9145, Longitude = 2.3838 },
            new() { Code = "CHAMPIGNY", Nom = "Champigny-sur-Marne", Departement = "Val-de-Marne", Region = "Île-de-France", Latitude = 48.8169, Longitude = 2.5145 },
            new() { Code = "ANTIBES", Nom = "Antibes", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5808, Longitude = 7.1251 },
            new() { Code = "CANNES", Nom = "Cannes", Departement = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5528, Longitude = 7.0174 },
            new() { Code = "BAGNEUX", Nom = "Bagneux", Departement = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.7957, Longitude = 2.3149 },
            new() { Code = "DRANCY", Nom = "Drancy", Departement = "Seine-Saint-Denis", Region = "Île-de-France", Latitude = 48.9282, Longitude = 2.4454 },
            new() { Code = "CARCES", Nom = "Carcès", Departement = "Var", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.4725, Longitude = 6.1836 }
        };

        await context.Villes.AddRangeAsync(villes);
    }
}