using System.Text.Json.Serialization;

namespace VehicleSeeder.Models;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    [JsonPropertyName("total")]   public int     Total   { get; set; }
    [JsonPropertyName("page")]    public int     Page    { get; set; }
    [JsonPropertyName("results")] public List<T> Results { get; set; } = [];
}