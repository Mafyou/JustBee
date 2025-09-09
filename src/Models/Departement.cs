namespace JustBeeWeb.Models;

public class Departement
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<Person> Persons { get; set; } = [];
}