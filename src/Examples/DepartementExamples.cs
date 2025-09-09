using JustBeeWeb.Models;
using JustBeeWeb.Services;

namespace JustBeeWeb.Examples;

/// <summary>
/// Classe d'exemple montrant comment utiliser le service des d�partements
/// pour ajouter des personnes dans diff�rents d�partements fran�ais
/// </summary>
public static class DepartementExamples
{
    /// <summary>
    /// Exemple d'ajout de personnes dans diff�rents d�partements
    /// </summary>
    public static void ExempleAjoutPersonnes()
    {
        var service = new DepartementService();

        // Ajouter une personne � Paris
        service.AddPersonToDepartement("75", new Person 
        { 
            Id = 1, 
            Pseudo = "Alice_Paris" 
        });

        // Ajouter plusieurs personnes � Lyon (Rh�ne)
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

        // Ajouter une personne � Marseille (Bouches-du-Rh�ne)
        service.AddPersonToDepartement("13", new Person 
        { 
            Id = 4, 
            Pseudo = "Diana_Marseille" 
        });

        // R�cup�rer un d�partement sp�cifique
        var rhone = service.GetDepartementByCode("69");
        if (rhone != null)
        {
            Console.WriteLine($"D�partement: {rhone.Nom}");
            Console.WriteLine($"R�gion: {rhone.Region}");
            Console.WriteLine($"Nombre de personnes: {rhone.Persons.Count}");
            foreach (var person in rhone.Persons)
            {
                Console.WriteLine($"- {person.Pseudo} (Lat: {person.Latitude}, Lon: {person.Longitude})");
            }
        }

        // R�cup�rer tous les d�partements avec des personnes
        var departementsAvecPersonnes = service.GetAllDepartements()
            .Where(d => d.Persons.Count != 0)
            .ToList();

        Console.WriteLine($"\nNombre de d�partements avec des personnes: {departementsAvecPersonnes.Count}");
    }

    /// <summary>
    /// G�n�re des donn�es d'exemple pour la d�monstration
    /// </summary>
    /// <param name="service">Service des d�partements</param>
    public static void GenererDonneesDemo(DepartementService service)
    {
        // Donn�es pour Paris et r�gion parisienne
        service.AddPersonToDepartement("75", new Person { Id = 1, Pseudo = "Jean_Paris" });
        service.AddPersonToDepartement("75", new Person { Id = 2, Pseudo = "Marie_Paris" });
        service.AddPersonToDepartement("92", new Person { Id = 3, Pseudo = "Pierre_Hauts92" });
        service.AddPersonToDepartement("93", new Person { Id = 4, Pseudo = "Sophie_Seine93" });

        // Donn�es pour Lyon et r�gion
        service.AddPersonToDepartement("69", new Person { Id = 5, Pseudo = "Luc_Lyon" });
        service.AddPersonToDepartement("69", new Person { Id = 6, Pseudo = "Emma_Lyon" });
        service.AddPersonToDepartement("01", new Person { Id = 7, Pseudo = "Tom_Ain" });

        // Donn�es pour Marseille et PACA
        service.AddPersonToDepartement("13", new Person { Id = 8, Pseudo = "Julie_Marseille" });
        service.AddPersonToDepartement("06", new Person { Id = 9, Pseudo = "Marc_Nice" });
        service.AddPersonToDepartement("83", new Person { Id = 10, Pseudo = "Laura_Var" });

        // Donn�es pour le Nord
        service.AddPersonToDepartement("59", new Person { Id = 11, Pseudo = "Paul_Lille" });
        service.AddPersonToDepartement("62", new Person { Id = 12, Pseudo = "Camille_Calais" });

        // Donn�es pour Toulouse et r�gion
        service.AddPersonToDepartement("31", new Person { Id = 13, Pseudo = "Alex_Toulouse" });
        service.AddPersonToDepartement("82", new Person { Id = 14, Pseudo = "Nina_Montauban" });

        // Donn�es pour Bordeaux et r�gion
        service.AddPersonToDepartement("33", new Person { Id = 15, Pseudo = "Hugo_Bordeaux" });
        service.AddPersonToDepartement("24", new Person { Id = 16, Pseudo = "L�a_Dordogne" });

        // Donn�es pour la Bretagne
        service.AddPersonToDepartement("35", new Person { Id = 17, Pseudo = "Yann_Rennes" });
        service.AddPersonToDepartement("29", new Person { Id = 18, Pseudo = "Morgane_Brest" });

        // Donn�es pour l'Est
        service.AddPersonToDepartement("67", new Person { Id = 19, Pseudo = "Klaus_Strasbourg" });
        service.AddPersonToDepartement("54", new Person { Id = 20, Pseudo = "Fran�ois_Nancy" });
    }
}