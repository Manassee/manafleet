// See https://aka.ms/new-console-template for more information

using VehicleSeeder.Api;
using VehicleSeeder.Data;

var carBrands = new[]
{
    "Audi", "BMW", "Mercedes-Benz", "Volkswagen", "Porsche",
    "Toyota", "Honda", "Ford", "Hyundai", "Renault",
    "Tesla", "Volvo", "Fiat", "Nissan", "Skoda", "Mazda",
    "Subaru", "Kia", "Tesla","Mitsubishi","Isuzu","Land Rover","BYD"
};

var motoBrands = new[]
{
    "Honda", "Yamaha", "Kawasaki", "Suzuki",
    "Ducati", "BMW", "KTM", "Triumph", "Harley-Davidson"
};

Console.WriteLine("╔══════════════════════════════════════╗");
Console.WriteLine("║      VehicleSeeder — ManaFleet       ║");
Console.WriteLine("╚══════════════════════════════════════╝\n");

var api = new VehicleApiClient();

await using var db = new VehicleDbContext();
await db.Database.EnsureCreatedAsync();
Console.WriteLine("✅ Database ready.\n");

// ── Cars ──────────────────────────────────────────────────
Console.WriteLine("🚗 Loading cars...\n");
foreach (var brand in carBrands)
{
    // Prüfen ob diese Marke schon in der DB ist
    bool alreadyExists = db.Cars.Any(c => c.Brand == brand);
    if (alreadyExists)
    {
        Console.WriteLine($"   {brand,-20} → skipped (already in DB)");
        continue;
    }
    
    
    var cars = await api.FetchCarsAsync(brand);
    await db.Cars.AddRangeAsync(cars);
    await db.SaveChangesAsync();
    Console.WriteLine($"   {brand,-20} → {cars.Count} entries");
    await Task.Delay(3500);
}

// ── Motorcycles ───────────────────────────────────────────
Console.WriteLine("\n🏍️  Loading motorcycles...\n");
foreach (var brand in motoBrands)
{
    bool alreadyExists = db.Motorcycles.Any(m => m.Brand == brand);
    if (alreadyExists)
    {
        Console.WriteLine($"   {brand,-20} → skipped (already in DB)");
        continue;
    }
    
    var motos = await api.FetchMotorcyclesAsync(brand);
    await db.Motorcycles.AddRangeAsync(motos);
    await db.SaveChangesAsync();
    Console.WriteLine($"   {brand,-20} → {motos.Count} entries");
    await Task.Delay(3500);
}

// ── Summary ───────────────────────────────────────────────
Console.WriteLine($"""

                   ╔══════════════════════════════════════╗
                   ║         ✅ Import complete!          ║
                   ╠══════════════════════════════════════╣
                   ║  🚗 Cars:         {db.Cars.Count(),5} entries        ║
                   ║  🏍  Motorcycles: {db.Motorcycles.Count(),5} entries        ║
                   ║  💾 File:         vehicles.db        ║
                   ╚══════════════════════════════════════╝
                   """);