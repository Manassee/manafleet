using Microsoft.EntityFrameworkCore;
using VehicleSeeder.Models;

namespace VehicleSeeder.Data;

public class VehicleDbContext : DbContext
{
    public DbSet<Car>        Cars        { get; set; }
    public DbSet<Motorcycle> Motorcycles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options
            .UseSqlite("Data Source=vehicles.db")
            // Zeigt dir im Terminal welches SQL EF Core generiert
            // → hilfreich zum Verstehen, später in Produktion entfernen
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Car>(entity =>
        {
            entity.ToTable("cars");
            entity.HasKey(c => c.Id);

            // ── Indizes ───────────────────────────────────────────
            // Warum Indizes? Stell dir ein Buch vor:
            // Ohne Index = jede Seite durchblättern
            // Mit Index  = direkt zur richtigen Seite springen

            // Primäre Suchfelder → eigene Indizes
            entity.HasIndex(c => c.UnitNumber)
                  .IsUnique()
                  .HasFilter("[unit_number] IS NOT NULL")
                  .HasDatabaseName("ix_cars_unit_number");

            entity.HasIndex(c => c.LicensePlate)
                  .IsUnique()
                  .HasFilter("[license_plate] IS NOT NULL")
                  .HasDatabaseName("ix_cars_license_plate");

            // Häufige Filter in Flottenanwendungen → eigene Indizes
            entity.HasIndex(c => c.Brand)
                  .HasDatabaseName("ix_cars_brand");

            entity.HasIndex(c => c.FuelType)
                  .HasDatabaseName("ix_cars_fuel_type");

            // Kombinations-Index: wenn du oft nach Brand + Year filterst
            // z.B. "alle BMW ab 2020"
            entity.HasIndex(c => new { c.Brand, c.Year })
                  .HasDatabaseName("ix_cars_brand_year");

            // ── Spalten ───────────────────────────────────────────
            entity.Property(c => c.Id)
                  .HasColumnName("id")
                  .ValueGeneratedNever(); // GUID wird in C# generiert, nicht von der DB

            entity.Property(c => c.UnitNumber)
                  .HasColumnName("unit_number")
                  .HasMaxLength(20);  // begrenzte Länge → weniger Speicher

            entity.Property(c => c.LicensePlate)
                  .HasColumnName("license_plate")
                  .HasMaxLength(20);

            entity.Property(c => c.Brand)
                  .HasColumnName("brand")
                  .HasMaxLength(100)
                  .IsRequired(false);

            entity.Property(c => c.Model)
                  .HasColumnName("model")
                  .HasMaxLength(100)
                  .IsRequired(false);

            entity.Property(c => c.Year)        .HasColumnName("year");
            entity.Property(c => c.Trim)        .HasColumnName("trim")       .HasMaxLength(100);
            entity.Property(c => c.Segment)     .HasColumnName("segment")    .HasMaxLength(50);
            entity.Property(c => c.BodyType)    .HasColumnName("body_type")  .HasMaxLength(50);
            entity.Property(c => c.FuelType)    .HasColumnName("fuel_type")  .HasMaxLength(30);
            entity.Property(c => c.PowerHp)     .HasColumnName("power_hp");
            entity.Property(c => c.Seats)       .HasColumnName("seats");
        });

        model.Entity<Motorcycle>(entity =>
        {
            entity.ToTable("motorcycles");
            entity.HasKey(m => m.Id);

            entity.HasIndex(m => m.UnitNumber)
                  .IsUnique()
                  .HasFilter("[unit_number] IS NOT NULL")
                  .HasDatabaseName("ix_motorcycles_unit_number");

            entity.HasIndex(m => m.LicensePlate)
                  .IsUnique()
                  .HasFilter("[license_plate] IS NOT NULL")
                  .HasDatabaseName("ix_motorcycles_license_plate");

            entity.HasIndex(m => m.Brand)
                  .HasDatabaseName("ix_motorcycles_brand");

            entity.HasIndex(m => new { m.Brand, m.Year })
                  .HasDatabaseName("ix_motorcycles_brand_year");

            entity.Property(m => m.Id)
                  .HasColumnName("id")
                  .ValueGeneratedNever();

            entity.Property(m => m.UnitNumber)  .HasColumnName("unit_number")   .HasMaxLength(20);
            entity.Property(m => m.LicensePlate).HasColumnName("license_plate") .HasMaxLength(20);
            entity.Property(m => m.Brand)       .HasColumnName("brand")         .HasMaxLength(100);
            entity.Property(m => m.Model)       .HasColumnName("model")         .HasMaxLength(100);
            entity.Property(m => m.Year)        .HasColumnName("year");
            entity.Property(m => m.Trim)        .HasColumnName("trim")          .HasMaxLength(100);
            entity.Property(m => m.Category)    .HasColumnName("category")      .HasMaxLength(50);
            entity.Property(m => m.FuelType)    .HasColumnName("fuel_type")     .HasMaxLength(30);
            entity.Property(m => m.PowerHp)     .HasColumnName("power_hp");
        });
    }
}