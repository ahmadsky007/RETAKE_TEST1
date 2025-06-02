namespace WebApplication1.Models;

public class Client
{
    public int ID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public List<CarRental>? Rentals { get; set; }
}