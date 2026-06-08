using System.Net.Http.Json;
using VehicleSeeder.Models;

namespace VehicleSeeder.Api;

public class VehicleApiClient
{
    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("https://api.carsdataset.com"),
        Timeout     = TimeSpan.FromSeconds(15)
    };

    private const string Endpoint = "/api/v1/preview/search";
    private const int    PageSize = 20;

    public Task<List<Car>>        FetchCarsAsync(string brand)
        => FetchAllAsync<Car>(brand);

    public Task<List<Motorcycle>> FetchMotorcyclesAsync(string brand)
        => FetchAllAsync<Motorcycle>(brand);

    private static async Task<List<T>> FetchAllAsync<T>(string brand)
    {
        var all  = new List<T>();
        int page = 1;

        while (true)
        {
            try
            {
                var url = $"{Endpoint}" +
                          $"?brand={Uri.EscapeDataString(brand)}" +
                          $"&page={page}" +
                          $"&page_size={PageSize}";

                var response = await Client.GetFromJsonAsync<ApiResponse<T>>(url);

                if (response?.Results is null or { Count: 0 })
                    break;

                all.AddRange(response.Results);

                int totalPages = (int)Math.Ceiling(response.Total / (double)PageSize);
                if (page >= totalPages || response.Results.Count < PageSize)
                    break;

                page++;
                await Task.Delay(2500);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"  Network error [{brand}] page {page}: {ex.Message}");
                break;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"  Timeout [{brand}] page {page}");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Unexpected error [{brand}]: {ex.Message}");
                break;
            }
        }

        return all;
    }
}