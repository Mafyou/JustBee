using JustBeeInfrastructure.Repositories;
using JustBeeInfrastructure.Models;

namespace JustBeeWeb.Services;

public class AlveoleService(IAlveoleRepository alveoleRepository)
{
    private readonly IAlveoleRepository _alveoleRepository = alveoleRepository;

    public async Task<List<Alveole>> GetAllAlveolesAsync() => 
        (await _alveoleRepository.GetAllAsync()).ToList();

    public async Task<List<Alveole>> GetAlveolesVerifieesAsync() =>
        (await _alveoleRepository.GetVerifiedAsync()).ToList();

    public async Task<List<Alveole>> GetAlveolesByVilleAsync(string villeCode) =>
        (await _alveoleRepository.GetByVilleCodeAsync(villeCode)).ToList();

    public async Task<Alveole?> GetAlveoleByIdAsync(int id) =>
        await _alveoleRepository.GetByIdAsync(id);

    public async Task<Alveole?> GetAlveoleByTokenAsync(string token) =>
        await _alveoleRepository.GetByTokenAsync(token);

    public async Task<bool> AjouterAlveoleAsync(Alveole alveole)
    {
        try
        {
            await _alveoleRepository.AddAsync(alveole);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> VerifierAlveoleAsync(string token) =>
        await _alveoleRepository.VerifyEmailAsync(token);

    public async Task<bool> SupprimerAlveoleAsync(int id) =>
        await _alveoleRepository.DeleteAsync(id);

    public async Task<Dictionary<string, int>> GetStatistiquesAlveolesAsync() =>
        await _alveoleRepository.GetStatisticsAsync();

    // Synchronous methods for backward compatibility (will be removed later)
    public List<Alveole> GetAllAlveoles() => GetAllAlveolesAsync().Result;
    public List<Alveole> GetAlveolesVerifiees() => GetAlveolesVerifieesAsync().Result;
    public List<Alveole> GetAlveolesByVille(string villeCode) => GetAlveolesByVilleAsync(villeCode).Result;
    public Alveole? GetAlveoleById(int id) => GetAlveoleByIdAsync(id).Result;
    public Alveole? GetAlveoleByToken(string token) => GetAlveoleByTokenAsync(token).Result;
    public void AjouterAlveole(Alveole alveole) => AjouterAlveoleAsync(alveole).Wait();
    public bool VerifierAlveole(string token) => VerifierAlveoleAsync(token).Result;
    public bool SupprimerAlveole(int id) => SupprimerAlveoleAsync(id).Result;
    public Dictionary<string, int> GetStatistiquesAlveoles() => GetStatistiquesAlveolesAsync().Result;
}