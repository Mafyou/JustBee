using JustBeeInfrastructure.Models;

namespace JustBeeInfrastructure.Repositories;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<IEnumerable<Person>> GetVerifiedAsync();
    Task<Person?> GetByIdAsync(int id);
    Task<Person?> GetByTokenAsync(string token);
    Task<Person?> GetByEmailAsync(string email);
    Task<IEnumerable<Person>> GetByVilleCodeAsync(string villeCode);
    Task<Person> AddAsync(Person person);
    Task<Person> UpdateAsync(Person person);
    Task<bool> DeleteAsync(int id);
    Task<bool> VerifyEmailAsync(string token);
}