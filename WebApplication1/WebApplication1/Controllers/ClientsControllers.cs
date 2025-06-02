using WebApplication1.DTO;
using WebApplication1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null)
            return NotFound();

        return Ok(client);
    }

    // POST api/clients
    [HttpPost]
    public async Task<IActionResult> AddClientWithRental([FromBody] CreateClientRequest request)
    {
        var success = await _clientService.AddClientWithRentalAsync(request);
        if (!success)
            return BadRequest("Car with given ID does not exist.");

        return Created("", null);
    }
}