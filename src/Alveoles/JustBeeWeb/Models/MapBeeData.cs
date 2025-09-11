using JustBeeWeb.Serialization;

namespace JustBeeWeb.Models;

/// <summary>
/// Data structure for caching MapBee page data
/// </summary>
public record MapBeeData
{
    public List<VilleCacheDto> Villes { get; init; } = [];
    public List<PersonCacheDto> AllPersons { get; init; } = [];
    public List<AlveoleCacheDto> AllAlveoles { get; init; } = [];
    public List<DepartementCacheDto> Departements { get; init; } = [];
    public Dictionary<string, List<PersonCacheDto>> PersonsByDepartement { get; init; } = [];
}