using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace JustBeeInfrastructure.Repositories;

public class PersonRepository(JustBeeContext context) : IPersonRepository
{
    private readonly JustBeeContext _context = context;

    public async Task<IEnumerable<Person>> GetAllAsync() =>
        await _context.Persons.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Person>> GetVerifiedAsync() =>
        await _context.Persons
            .AsNoTracking()
            .Where(p => p.EmailVerifie)
            .ToListAsync();

    public async Task<Person?> GetByIdAsync(int id) =>
        await _context.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Person?> GetByTokenAsync(string token) =>
        await _context.Persons.FirstOrDefaultAsync(p => p.TokenVerification == token);

    public async Task<Person?> GetByEmailAsync(string email) =>
        await _context.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.Email == email);

    public async Task<IEnumerable<Person>> GetByVilleCodeAsync(string villeCode) =>
        await _context.Persons
            .AsNoTracking()
            .Where(p => p.VilleCode == villeCode)
            .ToListAsync();

    public async Task<Person> AddAsync(Person person)
    {
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task<Person> UpdateAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var person = await GetByIdAsync(id);
        if (person == null) return false;

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var person = await GetByTokenAsync(token);
        if (person == null || person.EmailVerifie) return false;

        person.EmailVerifie = true;
        person.DateVerification = DateTime.UtcNow;
        person.TokenVerification = null;

        await _context.SaveChangesAsync();
        return true;
    }
}