namespace WebApplication1.Models;

public class Car
{
    public int ID { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Seats { get; set; }
    public int PricePerDay { get; set; }
    public int ModelID { get; set; }
    public int ColorID { get; set; }

    public Model? Model { get; set; }
    public Color? Color { get; set; }
}