namespace JustBeeWeb.Models;

public class Person
{
    public int Id { get; set; }
    public string Pseudo { get; set; } = string.Empty;
    public string? DepartementCode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
