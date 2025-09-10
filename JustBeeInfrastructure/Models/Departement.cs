namespace JustBeeInfrastructure.Models;

public class Departement
{
    public required string Code { get; set; }
    public required string Nom { get; set; }
    public required string Region { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}