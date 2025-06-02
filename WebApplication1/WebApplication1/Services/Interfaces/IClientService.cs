using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IClientService
{
    Task<ClientDto?> GetClientWithRentalsAsync(int clientId);
    Task<bool> CreateClientWithRentalAsync(CreateClientRequest request);
}