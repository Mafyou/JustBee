using JustBeeWeb.Models;

namespace JustBeeWeb.Services;

public class AlveoleService
{
    private static readonly List<Alveole> _alveoles = new();
    private static int _nextAlveoleId = 1;

    public List<Alveole> GetAllAlveoles() => _alveoles;

    public List<Alveole> GetAlveolesVerifiees() => 
        _alveoles.Where(a => a.EmailVerifie).ToList();

    public List<Alveole> GetAlveolesByVille(string villeCode) => 
        _alveoles.Where(a => a.VilleCode == villeCode && a.EmailVerifie).ToList();

    public Alveole? GetAlveoleById(int id) => 
        _alveoles.FirstOrDefault(a => a.Id == id);

    public Alveole? GetAlveoleByToken(string token) => 
        _alveoles.FirstOrDefault(a => a.TokenVerification == token);

    public void AjouterAlveole(Alveole alveole)
    {
        alveole.Id = _nextAlveoleId++;
        alveole.TokenVerification = Guid.NewGuid().ToString();
        alveole.DateCreation = DateTime.UtcNow;
        _alveoles.Add(alveole);
    }

    public bool VerifierAlveole(string token)
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

    public bool SupprimerAlveole(int id)
    {
        var alveole = GetAlveoleById(id);
        if (alveole != null)
        {
            _alveoles.Remove(alveole);
            return true;
        }
        return false;
    }

    public Dictionary<string, int> GetStatistiquesAlveoles()
    {
        var alveolesVerifiees = GetAlveolesVerifiees();
        return new Dictionary<string, int>
        {
            ["Total"] = alveolesVerifiees.Count,
            ["Par ville"] = alveolesVerifiees.GroupBy(a => a.VilleCode).Count()
        };
    }
}