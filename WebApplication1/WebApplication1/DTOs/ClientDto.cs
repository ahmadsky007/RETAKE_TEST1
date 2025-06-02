namespace WebApplication1.DTO;

public class ClientDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<CarRentalDto> Rentals { get; set; } = new();
}