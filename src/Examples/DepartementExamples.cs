using JustBeeWeb.Models;
using JustBeeWeb.Services;

namespace JustBeeWeb.Examples;

/// <summary>
/// Classe d'exemple montrant comment utiliser le service des départements
/// pour ajouter des personnes dans différents départements français
/// </summary>
public static class DepartementExamples
{
    /// <summary>
    /// Exemple d'ajout de personnes dans différents départements
    /// </summary>
    public static void ExempleAjoutPersonnes()
    {
        var service = new DepartementService();

        // Ajouter une personne à Paris
        service.AddPersonToDepartement("75", new Person 
        { 
            Id = 1, 
            Pseudo = "Alice_Paris" 
        });

        // Ajouter plusieurs personnes à Lyon (Rhône)
        service.AddPersonToDepartement("69", new Person 
        { 
            Id = 2, 
            Pseudo = "Bob_Lyon" 
        });
        
        service.AddPersonToDepartement("69", new Person 
        { 
            Id = 3, 
            Pseudo = "Charlie_Lyon" 
        });

        // Ajouter une personne à Marseille (Bouches-du-Rhône)
        service.AddPersonToDepartement("13", new Person 
        { 
            Id = 4, 
            Pseudo = "Diana_Marseille" 
        });

        // Récupérer un département spécifique
        var rhone = service.GetDepartementByCode("69");
        if (rhone != null)
        {
            Console.WriteLine($"Département: {rhone.Nom}");
            Console.WriteLine($"Région: {rhone.Region}");
            Console.WriteLine($"Nombre de personnes: {rhone.Persons.Count}");
            foreach (var person in rhone.Persons)
            {
                Console.WriteLine($"- {person.Pseudo} (Lat: {person.Latitude}, Lon: {person.Longitude})");
            }
        }

        // Récupérer tous les départements avec des personnes
        var departementsAvecPersonnes = service.GetAllDepartements()
            .Where(d => d.Persons.Count != 0)
            .ToList();

        Console.WriteLine($"\nNombre de départements avec des personnes: {departementsAvecPersonnes.Count}");
    }

    /// <summary>
    /// Génère des données d'exemple pour la démonstration
    /// </summary>
    /// <param name="service">Service des départements</param>
    public static void GenererDonneesDemo(DepartementService service)
    {
        // Données pour Paris et région parisienne
        service.AddPersonToDepartement("75", new Person { Id = 1, Pseudo = "Jean_Paris" });
        service.AddPersonToDepartement("75", new Person { Id = 2, Pseudo = "Marie_Paris" });
        service.AddPersonToDepartement("92", new Person { Id = 3, Pseudo = "Pierre_Hauts92" });
        service.AddPersonToDepartement("93", new Person { Id = 4, Pseudo = "Sophie_Seine93" });

        // Données pour Lyon et région
        service.AddPersonToDepartement("69", new Person { Id = 5, Pseudo = "Luc_Lyon" });
        service.AddPersonToDepartement("69", new Person { Id = 6, Pseudo = "Emma_Lyon" });
        service.AddPersonToDepartement("01", new Person { Id = 7, Pseudo = "Tom_Ain" });

        // Données pour Marseille et PACA
        service.AddPersonToDepartement("13", new Person { Id = 8, Pseudo = "Julie_Marseille" });
        service.AddPersonToDepartement("06", new Person { Id = 9, Pseudo = "Marc_Nice" });
        service.AddPersonToDepartement("83", new Person { Id = 10, Pseudo = "Laura_Var" });

        // Données pour le Nord
        service.AddPersonToDepartement("59", new Person { Id = 11, Pseudo = "Paul_Lille" });
        service.AddPersonToDepartement("62", new Person { Id = 12, Pseudo = "Camille_Calais" });

        // Données pour Toulouse et région
        service.AddPersonToDepartement("31", new Person { Id = 13, Pseudo = "Alex_Toulouse" });
        service.AddPersonToDepartement("82", new Person { Id = 14, Pseudo = "Nina_Montauban" });

        // Données pour Bordeaux et région
        service.AddPersonToDepartement("33", new Person { Id = 15, Pseudo = "Hugo_Bordeaux" });
        service.AddPersonToDepartement("24", new Person { Id = 16, Pseudo = "Léa_Dordogne" });

        // Données pour la Bretagne
        service.AddPersonToDepartement("35", new Person { Id = 17, Pseudo = "Yann_Rennes" });
        service.AddPersonToDepartement("29", new Person { Id = 18, Pseudo = "Morgane_Brest" });

        // Données pour l'Est
        service.AddPersonToDepartement("67", new Person { Id = 19, Pseudo = "Klaus_Strasbourg" });
        service.AddPersonToDepartement("54", new Person { Id = 20, Pseudo = "François_Nancy" });
    }
}