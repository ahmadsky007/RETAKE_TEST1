namespace WebApplication1.DTO;

public class CreateClientRequest
{
    public ClientData Client { get; set; } = null!;
    public int CarId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }

    public class ClientData
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}