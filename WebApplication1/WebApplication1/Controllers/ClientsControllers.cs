using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetClient(int clientId)
    {
        var client = await _clientService.GetClientWithRentalsAsync(clientId);
        if (client == null)
        {
            return NotFound($"Client with ID {clientId} not found.");
        }

        return Ok(client);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
    {
        var result = await _clientService.CreateClientWithRentalAsync(request);
        if (!result)
        {
            return BadRequest("Invalid data or car not found.");
        }

        return Created("", null); // Optionally add a location URI or ID
    }
}