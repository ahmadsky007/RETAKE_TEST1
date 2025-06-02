using Microsoft.Data.SqlClient;
using WebApplication1.DTOs;

namespace WebApplication1.Services;

public class ClientService : IClientService
{
    private readonly IConfiguration _configuration;

    public ClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ClientDto?> GetClientWithRentalsAsync(int clientId)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Get client data
        var command = new SqlCommand("SELECT * FROM clients WHERE ID = @id", connection);
        command.Parameters.AddWithValue("@id", clientId);

        using var reader = await command.ExecuteReaderAsync();
        if (!reader.Read()) return null;

        var client = new ClientDto
        {
            ID = (int)reader["ID"],
            FirstName = reader["FirstName"].ToString()!,
            LastName = reader["LastName"].ToString()!,
            Address = reader["Address"].ToString()!,
            Rentals = new List<CarRentalDto>()
        };

        await reader.CloseAsync();

        // Get rentals in a new using block
        command = new SqlCommand(@"
            SELECT c.VIN, col.Name AS Color, m.Name AS Model, r.DateFrom, r.DateTo, r.TotalPrice
            FROM car_rentals r
            JOIN cars c ON r.CarID = c.ID
            JOIN models m ON c.ModelID = m.ID
            JOIN colors col ON c.ColorID = col.ID
            WHERE r.ClientID = @clientId
        ", connection);
        command.Parameters.AddWithValue("@clientId", clientId);

        using var rentalReader = await command.ExecuteReaderAsync();
        while (await rentalReader.ReadAsync())
        {
            client.Rentals.Add(new CarRentalDto
            {
                VIN = rentalReader["VIN"].ToString()!,
                Color = rentalReader["Color"].ToString()!,
                Model = rentalReader["Model"].ToString()!,
                DateFrom = (DateTime)rentalReader["DateFrom"],
                DateTo = (DateTime)rentalReader["DateTo"],
                TotalPrice = (int)rentalReader["TotalPrice"]
            });
        }

        return client;
    }

    public async Task<bool> CreateClientWithRentalAsync(CreateClientRequest request)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Check if car exists
        var checkCarCmd = new SqlCommand("SELECT PricePerDay FROM cars WHERE ID = @carId", connection);
        checkCarCmd.Parameters.AddWithValue("@carId", request.CarId);
        var priceObj = await checkCarCmd.ExecuteScalarAsync();

        if (priceObj == null) return false;

        var pricePerDay = (int)priceObj;
        var totalDays = (request.DateTo - request.DateFrom).Days;
        var totalPrice = pricePerDay * totalDays;

        // Begin transaction
        var transaction = connection.BeginTransaction();
        try
        {
            // Insert client
            var insertClientCmd = new SqlCommand(@"
                INSERT INTO clients (FirstName, LastName, Address)
                OUTPUT INSERTED.ID
                VALUES (@firstName, @lastName, @address)", connection, transaction);
            insertClientCmd.Parameters.AddWithValue("@firstName", request.Client.FirstName);
            insertClientCmd.Parameters.AddWithValue("@lastName", request.Client.LastName);
            insertClientCmd.Parameters.AddWithValue("@address", request.Client.Address);

            var clientId = (int)await insertClientCmd.ExecuteScalarAsync();

            // Insert rental
            var insertRentalCmd = new SqlCommand(@"
                INSERT INTO car_rentals (ClientID, CarID, DateFrom, DateTo, TotalPrice, Discount)
                VALUES (@clientId, @carId, @dateFrom, @dateTo, @totalPrice, 0)", connection, transaction);
            insertRentalCmd.Parameters.AddWithValue("@clientId", clientId);
            insertRentalCmd.Parameters.AddWithValue("@carId", request.CarId);
            insertRentalCmd.Parameters.AddWithValue("@dateFrom", request.DateFrom);
            insertRentalCmd.Parameters.AddWithValue("@dateTo", request.DateTo);
            insertRentalCmd.Parameters.AddWithValue("@totalPrice", totalPrice);

            await insertRentalCmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}