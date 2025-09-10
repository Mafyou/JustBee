using JustBeeWeb.Models;

namespace JustBeeWeb.Services;

public class VilleService(VilleDataService? villeDataService = null)
{
    private static readonly List<Ville> _villes = InitializeVilles();
    private static int _nextPersonId = 1000; // Commencer à 1000 pour éviter les conflits avec les données de seed
    private readonly VilleDataService? _villeDataService = villeDataService;

    public List<Ville> GetAllVilles() => _villes;

    public async Task<List<Ville>> GetAllVillesFranceAsync()
    {
        if (_villeDataService != null)
        {
            return await _villeDataService.GetAllVillesFranceAsync();
        }
        return GetAllVilles();
    }

    public async Task<List<Ville>> SearchVillesAsync(string searchTerm)
    {
        if (_villeDataService != null)
        {
            return await _villeDataService.SearchVillesAsync(searchTerm);
        }

        // Fallback pour la recherche locale
        if (string.IsNullOrWhiteSpace(searchTerm))
            return GetAllVilles();

        var terme = searchTerm.ToLowerInvariant();
        return _villes
            .Where(v =>
                v.Nom.ToLowerInvariant().Contains(terme) ||
                v.Code.ToLowerInvariant().Contains(terme) ||
                v.Departement.ToLowerInvariant().Contains(terme) ||
                v.Region.ToLowerInvariant().Contains(terme))
            .ToList();
    }

    public Ville? GetVilleByCode(string code)
    {
        // Chercher d'abord dans les villes locales
        var ville = _villes.FirstOrDefault(v => v.Code == code);
        if (ville != null)
            return ville;

        // Si pas trouvé et qu'on a le service de données France, créer une ville temporaire
        // pour permettre la création d'alvéoles dans toutes les communes de France
        if (_villeDataService != null)
        {
            // Rechercher dans les données complètes et créer une entrée temporaire
            var villesFrance = _villeDataService.GetAllVillesFranceAsync().Result;
            var villeFrance = villesFrance.FirstOrDefault(v => v.Code == code);
            if (villeFrance != null)
            {
                // Créer une nouvelle ville avec les données et l'ajouter à notre liste
                var nouvelleVille = new Ville
                {
                    Code = villeFrance.Code,
                    Nom = villeFrance.Nom,
                    Departement = villeFrance.Departement,
                    Region = villeFrance.Region,
                    Latitude = villeFrance.Latitude,
                    Longitude = villeFrance.Longitude
                };
                _villes.Add(nouvelleVille);
                return nouvelleVille;
            }
        }

        return null;
    }

    public void AddPersonToVille(string villeCode, Person person)
    {
        var ville = GetVilleByCode(villeCode);
        if (ville is not null)
        {
            // Générer un ID automatiquement si pas fourni
            if (person.Id == 0)
            {
                person.Id = _nextPersonId++;
            }

            // Générer un token de vérification si l'email n'est pas encore vérifié
            if (!person.EmailVerifie && string.IsNullOrEmpty(person.TokenVerification))
            {
                person.TokenVerification = Guid.NewGuid().ToString();
            }

            person.VilleCode = villeCode;
            person.Latitude = ville.Latitude;
            person.Longitude = ville.Longitude;
            ville.Persons.Add(person);
        }
    }

    public void AddAlveoleToVille(string villeCode, Alveole alveole)
    {
        var ville = GetVilleByCode(villeCode);
        if (ville is not null)
        {
            alveole.VilleCode = villeCode;
            alveole.Latitude = ville.Latitude;
            alveole.Longitude = ville.Longitude;
            alveole.Ville = ville;
            ville.Alveoles.Add(alveole);
        }
    }

    public bool RemovePersonFromVille(string villeCode, int personId)
    {
        var ville = GetVilleByCode(villeCode);
        if (ville is not null)
        {
            var person = ville.Persons.FirstOrDefault(p => p.Id == personId);
            if (person is not null)
            {
                ville.Persons.Remove(person);
                return true;
            }
        }
        return false;
    }

    public bool RemoveAlveoleFromVille(string villeCode, int alveoleId)
    {
        var ville = GetVilleByCode(villeCode);
        if (ville is not null)
        {
            var alveole = ville.Alveoles.FirstOrDefault(a => a.Id == alveoleId);
            if (alveole is not null)
            {
                ville.Alveoles.Remove(alveole);
                return true;
            }
        }
        return false;
    }

    public List<Person> GetAllPersons() =>
        _villes.SelectMany(v => v.Persons).ToList();

    public List<Person> GetPersonsVerifiees() =>
        _villes.SelectMany(v => v.Persons).Where(p => p.EmailVerifie).ToList();

    public List<Alveole> GetAllAlveoles() =>
        _villes.SelectMany(v => v.Alveoles).ToList();

    public List<Alveole> GetAlveolesVerifiees() =>
        _villes.SelectMany(v => v.Alveoles).Where(a => a.EmailVerifie).ToList();

    public Person? GetPersonById(int id) =>
        _villes.SelectMany(v => v.Persons).FirstOrDefault(p => p.Id == id);

    public Person? GetPersonByToken(string token) =>
        _villes.SelectMany(v => v.Persons).FirstOrDefault(p => p.TokenVerification == token);

    public Alveole? GetAlveoleByToken(string token) =>
        _villes.SelectMany(v => v.Alveoles).FirstOrDefault(a => a.TokenVerification == token);

    public bool VerifierEmailPerson(string token)
    {
        var person = GetPersonByToken(token);
        if (person != null && !person.EmailVerifie)
        {
            person.EmailVerifie = true;
            person.DateVerification = DateTime.UtcNow;
            person.TokenVerification = null; // Supprimer le token après vérification
            return true;
        }
        return false;
    }

    public bool VerifierEmailAlveole(string token)
    {
        var alveole = GetAlveoleByToken(token);
        if (alveole != null && !alveole.EmailVerifie)
        {
            alveole.EmailVerifie = true;
            alveole.DateVerification = DateTime.UtcNow;
            alveole.TokenVerification = null; // Supprimer le token après vérification
            return true;
        }
        return false;
    }

    private static List<Ville> InitializeVilles() =>
        [
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
        ];
}