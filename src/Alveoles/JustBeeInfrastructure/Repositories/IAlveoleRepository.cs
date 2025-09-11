using JustBeeInfrastructure.Models;

namespace JustBeeInfrastructure.Repositories;

public interface IAlveoleRepository
{
    Task<IEnumerable<Alveole>> GetAllAsync();
    Task<IEnumerable<Alveole>> GetVerifiedAsync();
    Task<Alveole?> GetByIdAsync(int id);
    Task<Alveole?> GetByTokenAsync(string token);
    Task<Alveole?> GetByEmailAsync(string email);
    Task<IEnumerable<Alveole>> GetByVilleCodeAsync(string villeCode);
    Task<Alveole> AddAsync(Alveole alveole);
    Task<Alveole> UpdateAsync(Alveole alveole);
    Task<bool> DeleteAsync(int id);
    Task<bool> VerifyEmailAsync(string token);
    Task<Dictionary<string, int>> GetStatisticsAsync();
}