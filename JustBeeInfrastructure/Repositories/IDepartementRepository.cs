using JustBeeInfrastructure.Models;

namespace JustBeeInfrastructure.Repositories;

public interface IDepartementRepository
{
    Task<IEnumerable<Departement>> GetAllAsync();
    Task<Departement?> GetByCodeAsync(string code);
    Task<IEnumerable<Departement>> GetByRegionAsync(string region);
    Task<Departement> AddAsync(Departement departement);
    Task<Departement> UpdateAsync(Departement departement);
    Task<bool> DeleteAsync(string code);
    Task<bool> ExistsAsync(string code);
}