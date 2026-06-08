using System.Text.Json.Serialization;

namespace VehicleSeeder.Models;

public class Motorcycle
{
    public Guid Id {get; set;} 
    
    public string? UnitNumber {get; set;}
    public string? LicensePlate { get; set; }
    
    [JsonPropertyName("brand")]     public string? Brand    { get; set; }
    [JsonPropertyName("model")]     public string? Model    { get; set; }
    [JsonPropertyName("year")]      public int?    Year     { get; set; }
    [JsonPropertyName("trim")]      public string? Trim     { get; set; }
    [JsonPropertyName("category")]  public string? Category { get; set; }
    [JsonPropertyName("fuel_type")] public string? FuelType { get; set; }
    [JsonPropertyName("power_hp")]  public int?    PowerHp  { get; set; }
    
}