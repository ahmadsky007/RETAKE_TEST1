using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services.Implementations;

public class ClientService : IClientService
{
    private readonly CarRentalDbContext _context;

    public ClientService(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<ClientDto?> GetClientByIdAsync(int clientId)
    {
        var client = await _context.Clients
            .Include(c => c.CarRentals)
                .ThenInclude(r => r.Car)
                    .ThenInclude(car => car.Model)
            .Include(c => c.CarRentals)
                .ThenInclude(r => r.Car)
                    .ThenInclude(car => car.Color)
            .FirstOrDefaultAsync(c => c.Id == clientId);

        if (client == null) return null;

        return new ClientDto
        {
            Id = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Address = client.Address,
            Rentals = client.CarRentals.Select(r => new CarRentalDto
            {
                Vin = r.Car.Vin,
                Model = r.Car.Model.Name,
                Color = r.Car.Color.Name,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo,
                TotalPrice = r.TotalPrice
            }).ToList()
        };
    }

    public async Task<bool> AddClientWithRentalAsync(CreateClientRequest request)
    {
        var car = await _context.Cars.FindAsync(request.CarId);
        if (car == null) return false;

        var client = new Client
        {
            FirstName = request.Client.FirstName,
            LastName = request.Client.LastName,
            Address = request.Client.Address
        };

        var totalDays = (request.DateTo - request.DateFrom).Days;
        var totalPrice = totalDays * car.PricePerDay;

        var rental = new CarRental
        {
            Car = car,
            Client = client,
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            TotalPrice = totalPrice
        };

        _context.CarRentals.Add(rental);
        await _context.SaveChangesAsync();

        return true;
    }
}