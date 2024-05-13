using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class ServiceRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public ServiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<Service>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<Service>("SELECT * FROM Service");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Service> GetByIdAsync(int serviceId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<Service>(
                    "SELECT * FROM Service WHERE ServiceId = @ServiceId", new { ServiceId = serviceId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<Service>> GetServiceByUserProfileIdAsync(int userProfileId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<Service>(
                    "SELECT * FROM Service WHERE UserProfileId = @UserProfileId AND Deleted = 0", new { UserProfileId = userProfileId });
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Service> InsertAsync(Service serviceModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Service (UserProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, ServiceImage) VALUES (@UserProfileId, @ServiceCategoryId, @Name, @Description, @Price, @EstimatedDuration, @ServiceImage); SELECT LAST_INSERT_Id();", serviceModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Service> UpdateAsync(Service serviceUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Service SET ServiceCategoryId = @ServiceCategoryId, Name = @Name, Description = @Description, Price = @Price, EstimatedDuration = @EstimatedDuration, ServiceImage = @ServiceImage, LastUpdateDate = NOW() WHERE ServiceId = @ServiceId", serviceUpdateModel);
                return await GetByIdAsync(serviceUpdateModel.ServiceId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateServiceImageAsync(int id, string path)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                int affectedRows = await connection.ExecuteAsync("UPDATE Service SET ServiceImage = @ServiceImage, LastUpdateDate = NOW() WHERE ServiceId = @ServiceId", new { ServiceId = id, ServiceImage = path });
                return affectedRows > 0;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int serviceId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Service SET Active = @IsActive WHERE ServiceId = @ServiceId", new { IsActive = isActive, ServiceId = serviceId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int serviceId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Service SET Deleted = @IsDeleted WHERE ServiceId = @ServiceId", new { IsDeleted = isDeleted, ServiceId = serviceId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
