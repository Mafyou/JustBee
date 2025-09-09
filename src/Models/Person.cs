namespace JustBeeWeb.Models;

public class Person
{
    public int Id { get; set; }
    public required string Pseudo { get; set; }
    public string? DepartementCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
