namespace WebApplication1.DTO;

public class CarRentalDto
{
    public string Vin { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Model { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int TotalPrice { get; set; }
}