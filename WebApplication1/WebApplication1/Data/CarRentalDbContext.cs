using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data;

public class CarRentalDbContext : DbContext
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<Model> Models => Set<Model>();
    public DbSet<CarRental> CarRentals => Set<CarRental>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().ToTable("clients").HasKey(c => c.Id);
        modelBuilder.Entity<Car>().ToTable("cars").HasKey(c => c.Id);
        modelBuilder.Entity<Color>().ToTable("colors").HasKey(c => c.Id);
        modelBuilder.Entity<Model>().ToTable("models").HasKey(m => m.Id);
        modelBuilder.Entity<CarRental>().ToTable("car_rentals").HasKey(cr => cr.Id);

        modelBuilder.Entity<CarRental>()
            .HasOne(cr => cr.Client)
            .WithMany(c => c.CarRentals)
            .HasForeignKey(cr => cr.ClientId);

        modelBuilder.Entity<CarRental>()
            .HasOne(cr => cr.Car)
            .WithMany(c => c.CarRentals)
            .HasForeignKey(cr => cr.CarId);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Model)
            .WithMany(m => m.Cars)
            .HasForeignKey(c => c.ModelId);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Color)
            .WithMany(cl => cl.Cars)
            .HasForeignKey(c => c.ColorId);
    }
}