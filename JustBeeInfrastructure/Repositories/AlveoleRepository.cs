using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace JustBeeInfrastructure.Repositories;

public class AlveoleRepository(JustBeeContext context) : IAlveoleRepository
{
    private readonly JustBeeContext _context = context;

    public async Task<IEnumerable<Alveole>> GetAllAsync() =>
        await _context.Alveoles
            .AsNoTracking()
            .Include(a => a.Ville)
            .ToListAsync();

    public async Task<IEnumerable<Alveole>> GetVerifiedAsync() =>
        await _context.Alveoles
            .AsNoTracking()
            .Include(a => a.Ville)
            .Where(a => a.EmailVerifie)
            .ToListAsync();

    public async Task<Alveole?> GetByIdAsync(int id) =>
        await _context.Alveoles
            .AsNoTracking()
            .Include(a => a.Ville)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Alveole?> GetByTokenAsync(string token) =>
        await _context.Alveoles
            .Include(a => a.Ville)
            .FirstOrDefaultAsync(a => a.TokenVerification == token);

    public async Task<Alveole?> GetByEmailAsync(string email) =>
        await _context.Alveoles
            .AsNoTracking()
            .Include(a => a.Ville)
            .FirstOrDefaultAsync(a => a.Email == email);

    public async Task<IEnumerable<Alveole>> GetByVilleCodeAsync(string villeCode) =>
        await _context.Alveoles
            .AsNoTracking()
            .Include(a => a.Ville)
            .Where(a => a.VilleCode == villeCode && a.EmailVerifie)
            .ToListAsync();

    public async Task<Alveole> AddAsync(Alveole alveole)
    {
        alveole.TokenVerification = Guid.NewGuid().ToString();
        alveole.DateCreation = DateTime.UtcNow;

        _context.Alveoles.Add(alveole);
        await _context.SaveChangesAsync();
        return alveole;
    }

    public async Task<Alveole> UpdateAsync(Alveole alveole)
    {
        _context.Alveoles.Update(alveole);
        await _context.SaveChangesAsync();
        return alveole;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var alveole = await GetByIdAsync(id);
        if (alveole == null) return false;

        _context.Alveoles.Remove(alveole);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var alveole = await GetByTokenAsync(token);
        if (alveole == null || alveole.EmailVerifie) return false;

        alveole.EmailVerifie = true;
        alveole.DateVerification = DateTime.UtcNow;
        alveole.TokenVerification = null;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Dictionary<string, int>> GetStatisticsAsync()
    {
        var verified = await GetVerifiedAsync();
        var alveolesVerifiees = verified.ToList();
        
        return new Dictionary<string, int>
        {
            ["Total"] = alveolesVerifiees.Count,
            ["Par ville"] = alveolesVerifiees.GroupBy(a => a.VilleCode).Count()
        };
    }
}