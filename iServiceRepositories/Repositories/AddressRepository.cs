using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class AddressRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public AddressRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<Address>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<Address>("SELECT * FROM Address");
                return queryResult.ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Address> GetByIdAsync(int addressId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<Address>(
                    "SELECT * FROM Address WHERE AddressId = @AddressId", new { AddressId = addressId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Address> InsertAsync(Address addressModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Address (Street, Number, Neighborhood, AdditionalInfo, City, State, Country, PostalCode) VALUES (@Street, @Number, @Neighborhood, @AdditionalInfo, @City, @State, @Country, @PostalCode); SELECT LAST_INSERT_Id();", addressModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Address> UpdateAsync(Address addressUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Address SET Street = @Street, Number = @Number, Neighborhood = @Neighborhood, AdditionalInfo = @AdditionalInfo, City = @City, State = @State, Country = @Country, PostalCode = @PostalCode, LastUpdateDate = NOW() WHERE AddressId = @AddressId", addressUpdateModel);
                return await GetByIdAsync(addressUpdateModel.AddressId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int addressId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Address SET Active = @IsActive WHERE AddressId = @AddressId", new { IsActive = isActive, AddressId = addressId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int addressId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Address SET Deleted = @IsDeleted WHERE AddressId = @AddressId", new { IsDeleted = isDeleted, AddressId = addressId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
