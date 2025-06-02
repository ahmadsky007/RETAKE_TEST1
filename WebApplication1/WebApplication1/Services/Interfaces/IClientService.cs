using WebApplication1.DTO;

namespace WebApplication1.Services.Interfaces;

public interface IClientService
{
    Task<ClientDto?> GetClientByIdAsync(int clientId);
    Task<bool> AddClientWithRentalAsync(CreateClientRequest request);
}