using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace JustBeeInfrastructure.Repositories;

public class VilleRepository(JustBeeContext context) : IVilleRepository
{
    private readonly JustBeeContext _context = context;

    public async Task<IEnumerable<Ville>> GetAllAsync() =>
        await _context.Villes.ToListAsync();

    public async Task<Ville?> GetByCodeAsync(string code) =>
        await _context.Villes.FirstOrDefaultAsync(v => v.Code == code);

    public async Task<Ville?> GetByNameAsync(string name) =>
        await _context.Villes.FirstOrDefaultAsync(v => v.Nom == name);

    public async Task<IEnumerable<Ville>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var terme = searchTerm.ToLowerInvariant();
        return await _context.Villes
            .Where(v =>
                v.Nom.ToLower().Contains(terme) ||
                v.Code.ToLower().Contains(terme) ||
                v.Departement.ToLower().Contains(terme) ||
                v.Region.ToLower().Contains(terme))
            .ToListAsync();
    }

    public async Task<Ville> AddAsync(Ville ville)
    {
        _context.Villes.Add(ville);
        await _context.SaveChangesAsync();
        return ville;
    }

    public async Task<Ville> UpdateAsync(Ville ville)
    {
        _context.Villes.Update(ville);
        await _context.SaveChangesAsync();
        return ville;
    }

    public async Task<bool> DeleteAsync(string code)
    {
        var ville = await GetByCodeAsync(code);
        if (ville == null) return false;

        _context.Villes.Remove(ville);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string code) =>
        await _context.Villes.AnyAsync(v => v.Code == code);
}