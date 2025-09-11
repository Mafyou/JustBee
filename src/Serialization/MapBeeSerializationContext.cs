using System.Text.Json.Serialization;

namespace JustBeeWeb.Serialization;

/// <summary>
/// JSON source generator context for MapBee data serialization with trimming support
/// </summary>
[JsonSerializable(typeof(VilleMapData))]
[JsonSerializable(typeof(VilleMapData[]))]
[JsonSerializable(typeof(PersonMapData))]
[JsonSerializable(typeof(PersonMapData[]))]
[JsonSerializable(typeof(AlveoleMapData))]
[JsonSerializable(typeof(AlveoleMapData[]))]
// Add API response classes for VilleDataService
[JsonSerializable(typeof(CommuneApiResponse))]
[JsonSerializable(typeof(CommuneApiResponse[]))]
[JsonSerializable(typeof(DepartementApi))]
[JsonSerializable(typeof(RegionApi))]
[JsonSerializable(typeof(CentreApi))]
// Add cache-safe DTOs instead of domain models
[JsonSerializable(typeof(VilleCacheDto))]
[JsonSerializable(typeof(VilleCacheDto[]))]
[JsonSerializable(typeof(List<VilleCacheDto>))]
[JsonSerializable(typeof(PersonCacheDto))]
[JsonSerializable(typeof(PersonCacheDto[]))]
[JsonSerializable(typeof(List<PersonCacheDto>))]
[JsonSerializable(typeof(AlveoleCacheDto))]
[JsonSerializable(typeof(AlveoleCacheDto[]))]
[JsonSerializable(typeof(List<AlveoleCacheDto>))]
[JsonSerializable(typeof(DepartementCacheDto))]
[JsonSerializable(typeof(DepartementCacheDto[]))]
[JsonSerializable(typeof(List<DepartementCacheDto>))]
[JsonSerializable(typeof(Dictionary<string, List<PersonCacheDto>>))]
// Add API controller request/response types
[JsonSerializable(typeof(CreatePersonRequest))]
[JsonSerializable(typeof(CreateAlveoleRequest))]
[JsonSerializable(typeof(VilleSearchResult))]
[JsonSerializable(typeof(VilleSearchResult[]))]
[JsonSerializable(typeof(List<VilleSearchResult>))]
[JsonSerializable(typeof(VilleCountResponse))]
[JsonSerializable(typeof(ApiErrorResponse))]
// Add MapBeeData for caching
[JsonSerializable(typeof(MapBeeData))]
[JsonSourceGenerationOptions(WriteIndented = false, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class MapBeeSerializationContext : JsonSerializerContext
{
}

/// <summary>
/// Cache-safe DTO for Ville without navigation properties
/// </summary>
public record VilleCacheDto
{
    public required string Code { get; init; }
    public required string Nom { get; init; }
    public required string Region { get; init; }
    public required string Departement { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}

/// <summary>
/// Cache-safe DTO for Person without navigation properties
/// </summary>
public record PersonCacheDto
{
    public int Id { get; init; }
    public required string Pseudo { get; init; }
    public required string Email { get; init; }
    public bool EmailVerifie { get; init; }
    public string? VilleCode { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public DateTime DateCreation { get; init; }
    public DateTime? DateVerification { get; init; }
    public string? TokenVerification { get; init; }
}

/// <summary>
/// Cache-safe DTO for Alveole without navigation properties
/// </summary>
public record AlveoleCacheDto
{
    public int Id { get; init; }
    public required string Nom { get; init; }
    public required string Description { get; init; }
    public required string VilleCode { get; init; }
    public required string Email { get; init; }
    public bool EmailVerifie { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public DateTime DateCreation { get; init; }
    public DateTime? DateVerification { get; init; }
    public string? TokenVerification { get; init; }
}

/// <summary>
/// Cache-safe DTO for Departement without navigation properties
/// </summary>
public record DepartementCacheDto
{
    public required string Code { get; init; }
    public required string Nom { get; init; }
    public required string Region { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}

/// <summary>
/// Ville data for map display
/// </summary>
public class VilleMapData
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
    public string? Region { get; set; }
    public string? Departement { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int PersonCount { get; set; }
    public int AlveoleCount { get; set; }
    public int TotalCount { get; set; }
    public PersonMapData[]? Persons { get; set; }
    public AlveoleMapData[]? Alveoles { get; set; }
}

/// <summary>
/// Person data for map display
/// </summary>
public class PersonMapData
{
    public int Id { get; set; }
    public string? Pseudo { get; set; }
}

/// <summary>
/// Alveole data for map display
/// </summary>
public class AlveoleMapData
{
    public int Id { get; set; }
    public string? Nom { get; set; }
}

/// <summary>
/// API response classes for VilleDataService
/// </summary>
public class CommuneApiResponse
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
    public DepartementApi? Departement { get; set; }
    public RegionApi? Region { get; set; }
    public CentreApi? Centre { get; set; }
}

public class DepartementApi
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
}

public class RegionApi
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
}

public class CentreApi
{
    public string? Type { get; set; }
    public double[]? Coordinates { get; set; }
}

/// <summary>
/// API request types for controllers
/// </summary>
public class CreatePersonRequest
{
    public required string Pseudo { get; set; }
    public required string Email { get; set; }
}

public class CreateAlveoleRequest
{
    public required string Nom { get; set; }
    public string? Description { get; set; }
    public required string Email { get; set; }
}

/// <summary>
/// API response types for controllers
/// </summary>
public class VilleSearchResult
{
    public string? Code { get; set; }
    public string? Nom { get; set; }
    public string? Departement { get; set; }
    public string? Region { get; set; }
    public string? Display { get; set; }
}

public class VilleCountResponse
{
    public int Count { get; set; }
    public string? Message { get; set; }
}

public class ApiErrorResponse
{
    public string? Error { get; set; }
    public string? Details { get; set; }
}