namespace WebApplication1.Models;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;

    public ICollection<CarRental> CarRentals { get; set; } = new List<CarRental>();
}