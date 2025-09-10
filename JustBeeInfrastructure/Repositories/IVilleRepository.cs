using JustBeeInfrastructure.Models;

namespace JustBeeInfrastructure.Repositories;

public interface IVilleRepository
{
    Task<IEnumerable<Ville>> GetAllAsync();
    Task<Ville?> GetByCodeAsync(string code);
    Task<Ville?> GetByNameAsync(string name);
    Task<IEnumerable<Ville>> SearchAsync(string searchTerm);
    Task<Ville> AddAsync(Ville ville);
    Task<Ville> UpdateAsync(Ville ville);
    Task<bool> DeleteAsync(string code);
    Task<bool> ExistsAsync(string code);
}