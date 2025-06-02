namespace WebApplication1.DTOs;

public class ClientDto
{
    public int ID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<CarRentalDto> Rentals { get; set; } = new();
}