using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace JustBeeInfrastructure.Repositories;

public class DepartementRepository(JustBeeContext context) : IDepartementRepository
{
    private readonly JustBeeContext _context = context;

    public async Task<IEnumerable<Departement>> GetAllAsync() =>
        await _context.Departements.ToListAsync();

    public async Task<Departement?> GetByCodeAsync(string code) =>
        await _context.Departements.FirstOrDefaultAsync(d => d.Code == code);

    public async Task<IEnumerable<Departement>> GetByRegionAsync(string region) =>
        await _context.Departements
            .Where(d => d.Region == region)
            .ToListAsync();

    public async Task<Departement> AddAsync(Departement departement)
    {
        _context.Departements.Add(departement);
        await _context.SaveChangesAsync();
        return departement;
    }

    public async Task<Departement> UpdateAsync(Departement departement)
    {
        _context.Departements.Update(departement);
        await _context.SaveChangesAsync();
        return departement;
    }

    public async Task<bool> DeleteAsync(string code)
    {
        var departement = await GetByCodeAsync(code);
        if (departement == null) return false;

        _context.Departements.Remove(departement);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string code) =>
        await _context.Departements.AnyAsync(d => d.Code == code);
}