namespace WebApplication1.Models;

public class CarRental
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int CarId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int TotalPrice { get; set; }
    public int? Discount { get; set; }

    public Client Client { get; set; } = null!;
    public Car Car { get; set; } = null!;
}