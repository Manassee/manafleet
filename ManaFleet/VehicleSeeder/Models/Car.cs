using System.Text.Json.Serialization;

namespace VehicleSeeder.Models;

public class Car
{
    // unique generated Id 
    
    public Guid Id { get; set; }
    
    public string? UnitNumber { get; set; } //MF1997
    public string? LicensePlate { get; set; } // BO-MM1997
    
    [JsonPropertyName("brand")]     public string? Brand    { get; set; }
    [JsonPropertyName("model")]     public string? Model    { get; set; }
    [JsonPropertyName("year")]      public int?    Year     { get; set; }
    [JsonPropertyName("trim")]      public string? Trim     { get; set; }
    [JsonPropertyName("segment")]   public string? Segment  { get; set; }
    [JsonPropertyName("body_type")] public string? BodyType { get; set; }
    [JsonPropertyName("fuel_type")] public string? FuelType { get; set; }
    [JsonPropertyName("power_hp")]  public int?    PowerHp  { get; set; }
    [JsonPropertyName("seats")]     public int?    Seats    { get; set; }
}