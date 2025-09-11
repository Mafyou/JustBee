namespace JustBeeInfrastructure.Models;

public class Ville
{
    public required string Code { get; set; }
    public required string Nom { get; set; }
    public required string Region { get; set; }
    public required string Departement { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<Person> Persons { get; set; } = [];
    public List<Alveole> Alveoles { get; set; } = []; // Ajout des alvéoles
}