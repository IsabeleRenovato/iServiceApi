using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class ServiceCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public ServiceCategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<ServiceCategory>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<ServiceCategory>("SELECT * FROM ServiceCategory WHERE Deleted = 0");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<ServiceCategory> GetByIdAsync(int serviceCategoryId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<ServiceCategory>(
                    "SELECT * FROM ServiceCategory WHERE ServiceCategoryId = @ServiceCategoryId AND Deleted = 0", new { ServiceCategoryId = serviceCategoryId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<ServiceCategory>> GetByUserProfileIdAsync(int userProfileId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<ServiceCategory>(
                    "SELECT * FROM ServiceCategory WHERE UserProfileId = @UserProfileId AND Deleted = 0", new { UserProfileId = userProfileId });
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<ServiceCategory> GetByFilterAsync(int userProfileId, int serviceCategoryId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<ServiceCategory>(
                    "SELECT * FROM ServiceCategory WHERE ServiceCategoryId = @ServiceCategoryId AND UserProfileId = @UserProfileId", new { ServiceCategoryId = serviceCategoryId, UserProfileId = userProfileId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<ServiceCategory> InsertAsync(ServiceCategory serviceCategoryModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO ServiceCategory (UserProfileId, Name) VALUES (@UserProfileId, @Name); SELECT LAST_INSERT_Id();", serviceCategoryModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<ServiceCategory> UpdateAsync(ServiceCategory serviceCategoryUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE ServiceCategory SET Name = @Name, LastUpdateDate = NOW() WHERE ServiceCategoryId = @ServiceCategoryId", serviceCategoryUpdateModel);
                return await GetByIdAsync(serviceCategoryUpdateModel.ServiceCategoryId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int serviceCategoryId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE ServiceCategory SET Active = @IsActive WHERE ServiceCategoryId = @ServiceCategoryId", new { IsActive = isActive, ServiceCategoryId = serviceCategoryId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int serviceCategoryId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE ServiceCategory SET Deleted = @IsDeleted WHERE ServiceCategoryId = @ServiceCategoryId", new { IsDeleted = isDeleted, ServiceCategoryId = serviceCategoryId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
