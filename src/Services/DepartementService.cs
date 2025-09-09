using JustBeeWeb.Models;

namespace JustBeeWeb.Services;

public class DepartementService
{
    private static readonly List<Departement> _departements = InitializeDepartements();
    private static int _nextPersonId = 1000; // Commencer à 1000 pour éviter les conflits avec les données de seed

    public List<Departement> GetAllDepartements()
    {
        return _departements;
    }

    public Departement? GetDepartementByCode(string code)
    {
        return _departements.FirstOrDefault(d => d.Code == code);
    }

    public void AddPersonToDepartement(string departementCode, Person person)
    {
        var departement = GetDepartementByCode(departementCode);
        if (departement != null)
        {
            // Générer un ID automatiquement si pas fourni
            if (person.Id == 0)
            {
                person.Id = _nextPersonId++;
            }
            
            person.DepartementCode = departementCode;
            person.Latitude = departement.Latitude;
            person.Longitude = departement.Longitude;
            departement.Persons.Add(person);
        }
    }

    public bool RemovePersonFromDepartement(string departementCode, int personId)
    {
        var departement = GetDepartementByCode(departementCode);
        if (departement != null)
        {
            var person = departement.Persons.FirstOrDefault(p => p.Id == personId);
            if (person != null)
            {
                departement.Persons.Remove(person);
                return true;
            }
        }
        return false;
    }

    public List<Person> GetAllPersons()
    {
        return _departements.SelectMany(d => d.Persons).ToList();
    }

    public Person? GetPersonById(int id)
    {
        return _departements.SelectMany(d => d.Persons).FirstOrDefault(p => p.Id == id);
    }

    private static List<Departement> InitializeDepartements()
    {
        return
        [
            new Departement { Code = "01", Nom = "Ain", Region = "Auvergne-Rhône-Alpes", Latitude = 46.2, Longitude = 5.2 },
            new Departement { Code = "02", Nom = "Aisne", Region = "Hauts-de-France", Latitude = 49.4, Longitude = 3.4 },
            new Departement { Code = "03", Nom = "Allier", Region = "Auvergne-Rhône-Alpes", Latitude = 46.3, Longitude = 3.3 },
            new Departement { Code = "04", Nom = "Alpes-de-Haute-Provence", Region = "Provence-Alpes-Côte d'Azur", Latitude = 44.1, Longitude = 6.2 },
            new Departement { Code = "05", Nom = "Hautes-Alpes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 44.7, Longitude = 6.5 },
            new Departement { Code = "06", Nom = "Alpes-Maritimes", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.7, Longitude = 7.3 },
            new Departement { Code = "07", Nom = "Ardèche", Region = "Auvergne-Rhône-Alpes", Latitude = 44.7, Longitude = 4.6 },
            new Departement { Code = "08", Nom = "Ardennes", Region = "Grand Est", Latitude = 49.8, Longitude = 4.7 },
            new Departement { Code = "09", Nom = "Ariège", Region = "Occitanie", Latitude = 42.9, Longitude = 1.6 },
            new Departement { Code = "10", Nom = "Aube", Region = "Grand Est", Latitude = 48.3, Longitude = 4.1 },
            new Departement { Code = "11", Nom = "Aude", Region = "Occitanie", Latitude = 43.2, Longitude = 2.4 },
            new Departement { Code = "12", Nom = "Aveyron", Region = "Occitanie", Latitude = 44.4, Longitude = 2.6 },
            new Departement { Code = "13", Nom = "Bouches-du-Rhône", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5, Longitude = 5.4 },
            new Departement { Code = "14", Nom = "Calvados", Region = "Normandie", Latitude = 49.2, Longitude = -0.4 },
            new Departement { Code = "15", Nom = "Cantal", Region = "Auvergne-Rhône-Alpes", Latitude = 45.0, Longitude = 2.4 },
            new Departement { Code = "16", Nom = "Charente", Region = "Nouvelle-Aquitaine", Latitude = 45.7, Longitude = 0.2 },
            new Departement { Code = "17", Nom = "Charente-Maritime", Region = "Nouvelle-Aquitaine", Latitude = 45.7, Longitude = -0.8 },
            new Departement { Code = "18", Nom = "Cher", Region = "Centre-Val de Loire", Latitude = 47.1, Longitude = 2.4 },
            new Departement { Code = "19", Nom = "Corrèze", Region = "Nouvelle-Aquitaine", Latitude = 45.3, Longitude = 2.0 },
            new Departement { Code = "2A", Nom = "Corse-du-Sud", Region = "Corse", Latitude = 41.9, Longitude = 9.0 },
            new Departement { Code = "2B", Nom = "Haute-Corse", Region = "Corse", Latitude = 42.4, Longitude = 9.2 },
            new Departement { Code = "21", Nom = "Côte-d'Or", Region = "Bourgogne-Franche-Comté", Latitude = 47.3, Longitude = 4.8 },
            new Departement { Code = "22", Nom = "Côtes-d'Armor", Region = "Bretagne", Latitude = 48.4, Longitude = -2.8 },
            new Departement { Code = "23", Nom = "Creuse", Region = "Nouvelle-Aquitaine", Latitude = 46.0, Longitude = 2.0 },
            new Departement { Code = "24", Nom = "Dordogne", Region = "Nouvelle-Aquitaine", Latitude = 45.2, Longitude = 0.7 },
            new Departement { Code = "25", Nom = "Doubs", Region = "Bourgogne-Franche-Comté", Latitude = 47.2, Longitude = 6.1 },
            new Departement { Code = "26", Nom = "Drôme", Region = "Auvergne-Rhône-Alpes", Latitude = 44.7, Longitude = 5.1 },
            new Departement { Code = "27", Nom = "Eure", Region = "Normandie", Latitude = 49.0, Longitude = 1.2 },
            new Departement { Code = "28", Nom = "Eure-et-Loir", Region = "Centre-Val de Loire", Latitude = 48.4, Longitude = 1.5 },
            new Departement { Code = "29", Nom = "Finistère", Region = "Bretagne", Latitude = 48.2, Longitude = -4.1 },
            new Departement { Code = "30", Nom = "Gard", Region = "Occitanie", Latitude = 44.0, Longitude = 4.2 },
            new Departement { Code = "31", Nom = "Haute-Garonne", Region = "Occitanie", Latitude = 43.4, Longitude = 1.4 },
            new Departement { Code = "32", Nom = "Gers", Region = "Occitanie", Latitude = 43.7, Longitude = 0.6 },
            new Departement { Code = "33", Nom = "Gironde", Region = "Nouvelle-Aquitaine", Latitude = 44.8, Longitude = -0.6 },
            new Departement { Code = "34", Nom = "Hérault", Region = "Occitanie", Latitude = 43.6, Longitude = 3.9 },
            new Departement { Code = "35", Nom = "Ille-et-Vilaine", Region = "Bretagne", Latitude = 48.1, Longitude = -1.7 },
            new Departement { Code = "36", Nom = "Indre", Region = "Centre-Val de Loire", Latitude = 46.7, Longitude = 1.6 },
            new Departement { Code = "37", Nom = "Indre-et-Loire", Region = "Centre-Val de Loire", Latitude = 47.4, Longitude = 0.7 },
            new Departement { Code = "38", Nom = "Isère", Region = "Auvergne-Rhône-Alpes", Latitude = 45.4, Longitude = 5.6 },
            new Departement { Code = "39", Nom = "Jura", Region = "Bourgogne-Franche-Comté", Latitude = 46.7, Longitude = 5.8 },
            new Departement { Code = "40", Nom = "Landes", Region = "Nouvelle-Aquitaine", Latitude = 44.0, Longitude = -0.8 },
            new Departement { Code = "41", Nom = "Loir-et-Cher", Region = "Centre-Val de Loire", Latitude = 47.6, Longitude = 1.3 },
            new Departement { Code = "42", Nom = "Loire", Region = "Auvergne-Rhône-Alpes", Latitude = 45.4, Longitude = 4.4 },
            new Departement { Code = "43", Nom = "Haute-Loire", Region = "Auvergne-Rhône-Alpes", Latitude = 45.0, Longitude = 3.9 },
            new Departement { Code = "44", Nom = "Loire-Atlantique", Region = "Pays de la Loire", Latitude = 47.2, Longitude = -1.6 },
            new Departement { Code = "45", Nom = "Loiret", Region = "Centre-Val de Loire", Latitude = 47.9, Longitude = 2.1 },
            new Departement { Code = "46", Nom = "Lot", Region = "Occitanie", Latitude = 44.4, Longitude = 1.4 },
            new Departement { Code = "47", Nom = "Lot-et-Garonne", Region = "Nouvelle-Aquitaine", Latitude = 44.2, Longitude = 0.6 },
            new Departement { Code = "48", Nom = "Lozère", Region = "Occitanie", Latitude = 44.5, Longitude = 3.5 },
            new Departement { Code = "49", Nom = "Maine-et-Loire", Region = "Pays de la Loire", Latitude = 47.5, Longitude = -0.9 },
            new Departement { Code = "50", Nom = "Manche", Region = "Normandie", Latitude = 49.1, Longitude = -1.3 },
            new Departement { Code = "51", Nom = "Marne", Region = "Grand Est", Latitude = 49.0, Longitude = 4.0 },
            new Departement { Code = "52", Nom = "Haute-Marne", Region = "Grand Est", Latitude = 48.1, Longitude = 5.3 },
            new Departement { Code = "53", Nom = "Mayenne", Region = "Pays de la Loire", Latitude = 48.3, Longitude = -0.6 },
            new Departement { Code = "54", Nom = "Meurthe-et-Moselle", Region = "Grand Est", Latitude = 48.7, Longitude = 6.2 },
            new Departement { Code = "55", Nom = "Meuse", Region = "Grand Est", Latitude = 49.0, Longitude = 5.4 },
            new Departement { Code = "56", Nom = "Morbihan", Region = "Bretagne", Latitude = 47.7, Longitude = -2.7 },
            new Departement { Code = "57", Nom = "Moselle", Region = "Grand Est", Latitude = 49.1, Longitude = 6.7 },
            new Departement { Code = "58", Nom = "Nièvre", Region = "Bourgogne-Franche-Comté", Latitude = 47.0, Longitude = 3.5 },
            new Departement { Code = "59", Nom = "Nord", Region = "Hauts-de-France", Latitude = 50.5, Longitude = 3.2 },
            new Departement { Code = "60", Nom = "Oise", Region = "Hauts-de-France", Latitude = 49.4, Longitude = 2.8 },
            new Departement { Code = "61", Nom = "Orne", Region = "Normandie", Latitude = 48.6, Longitude = 0.1 },
            new Departement { Code = "62", Nom = "Pas-de-Calais", Region = "Hauts-de-France", Latitude = 50.4, Longitude = 2.6 },
            new Departement { Code = "63", Nom = "Puy-de-Dôme", Region = "Auvergne-Rhône-Alpes", Latitude = 45.8, Longitude = 3.1 },
            new Departement { Code = "64", Nom = "Pyrénées-Atlantiques", Region = "Nouvelle-Aquitaine", Latitude = 43.3, Longitude = -1.0 },
            new Departement { Code = "65", Nom = "Hautes-Pyrénées", Region = "Occitanie", Latitude = 43.2, Longitude = 0.1 },
            new Departement { Code = "66", Nom = "Pyrénées-Orientales", Region = "Occitanie", Latitude = 42.7, Longitude = 2.9 },
            new Departement { Code = "67", Nom = "Bas-Rhin", Region = "Grand Est", Latitude = 48.6, Longitude = 7.8 },
            new Departement { Code = "68", Nom = "Haut-Rhin", Region = "Grand Est", Latitude = 47.8, Longitude = 7.3 },
            new Departement { Code = "69", Nom = "Rhône", Region = "Auvergne-Rhône-Alpes", Latitude = 45.7, Longitude = 4.8 },
            new Departement { Code = "70", Nom = "Haute-Saône", Region = "Bourgogne-Franche-Comté", Latitude = 47.6, Longitude = 6.2 },
            new Departement { Code = "71", Nom = "Saône-et-Loire", Region = "Bourgogne-Franche-Comté", Latitude = 46.7, Longitude = 4.3 },
            new Departement { Code = "72", Nom = "Sarthe", Region = "Pays de la Loire", Latitude = 48.0, Longitude = 0.2 },
            new Departement { Code = "73", Nom = "Savoie", Region = "Auvergne-Rhône-Alpes", Latitude = 45.6, Longitude = 6.4 },
            new Departement { Code = "74", Nom = "Haute-Savoie", Region = "Auvergne-Rhône-Alpes", Latitude = 46.1, Longitude = 6.3 },
            new Departement { Code = "75", Nom = "Paris", Region = "Île-de-France", Latitude = 48.9, Longitude = 2.3 },
            new Departement { Code = "76", Nom = "Seine-Maritime", Region = "Normandie", Latitude = 49.4, Longitude = 1.1 },
            new Departement { Code = "77", Nom = "Seine-et-Marne", Region = "Île-de-France", Latitude = 48.5, Longitude = 2.9 },
            new Departement { Code = "78", Nom = "Yvelines", Region = "Île-de-France", Latitude = 48.8, Longitude = 2.0 },
            new Departement { Code = "79", Nom = "Deux-Sèvres", Region = "Nouvelle-Aquitaine", Latitude = 46.3, Longitude = -0.5 },
            new Departement { Code = "80", Nom = "Somme", Region = "Hauts-de-France", Latitude = 49.9, Longitude = 2.3 },
            new Departement { Code = "81", Nom = "Tarn", Region = "Occitanie", Latitude = 43.9, Longitude = 2.1 },
            new Departement { Code = "82", Nom = "Tarn-et-Garonne", Region = "Occitanie", Latitude = 44.0, Longitude = 1.4 },
            new Departement { Code = "83", Nom = "Var", Region = "Provence-Alpes-Côte d'Azur", Latitude = 43.5, Longitude = 6.5 },
            new Departement { Code = "84", Nom = "Vaucluse", Region = "Provence-Alpes-Côte d'Azur", Latitude = 44.0, Longitude = 5.1 },
            new Departement { Code = "85", Nom = "Vendée", Region = "Pays de la Loire", Latitude = 46.7, Longitude = -1.4 },
            new Departement { Code = "86", Nom = "Vienne", Region = "Nouvelle-Aquitaine", Latitude = 46.6, Longitude = 0.3 },
            new Departement { Code = "87", Nom = "Haute-Vienne", Region = "Nouvelle-Aquitaine", Latitude = 45.8, Longitude = 1.3 },
            new Departement { Code = "88", Nom = "Vosges", Region = "Grand Est", Latitude = 48.2, Longitude = 6.5 },
            new Departement { Code = "89", Nom = "Yonne", Region = "Bourgogne-Franche-Comté", Latitude = 47.8, Longitude = 3.6 },
            new Departement { Code = "90", Nom = "Territoire de Belfort", Region = "Bourgogne-Franche-Comté", Latitude = 47.6, Longitude = 6.9 },
            new Departement { Code = "91", Nom = "Essonne", Region = "Île-de-France", Latitude = 48.6, Longitude = 2.4 },
            new Departement { Code = "92", Nom = "Hauts-de-Seine", Region = "Île-de-France", Latitude = 48.8, Longitude = 2.2 },
            new Departement { Code = "93", Nom = "Seine-Saint-Denis", Region = "Île-de-France", Latitude = 48.9, Longitude = 2.5 },
            new Departement { Code = "94", Nom = "Val-de-Marne", Region = "Île-de-France", Latitude = 48.8, Longitude = 2.5 },
            new Departement { Code = "95", Nom = "Val-d'Oise", Region = "Île-de-France", Latitude = 49.0, Longitude = 2.1 }
        ];
    }
}