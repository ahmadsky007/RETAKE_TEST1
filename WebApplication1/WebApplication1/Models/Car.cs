namespace WebApplication1.Models;

public class Car
{
    public int Id { get; set; }
    public string Vin { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Seats { get; set; }
    public int PricePerDay { get; set; }
    public int ModelId { get; set; }
    public int ColorId { get; set; }

    public Model Model { get; set; } = null!;
    public Color Color { get; set; } = null!;
    public ICollection<CarRental> CarRentals { get; set; } = new List<CarRental>();
}