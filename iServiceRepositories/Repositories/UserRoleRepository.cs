using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class UserRoleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public UserRoleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<UserRole>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<UserRole>("SELECT * FROM UserRole");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<UserRole> GetByIdAsync(int userRoleId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<UserRole>(
                    "SELECT * FROM UserRole WHERE UserRoleId = @UserRoleId", new { UserRoleId = userRoleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<UserRole> InsertAsync(UserRole userRoleModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO UserRole (Name) VALUES (@Name); SELECT LAST_INSERT_Id();", userRoleModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<UserRole> UpdateAsync(UserRole userRoleUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE UserRole SET Name = @Name, LastUpdateDate = NOW() WHERE UserRoleId = @UserRoleId", userRoleUpdateModel);
                return await GetByIdAsync(userRoleUpdateModel.UserRoleId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int userRoleId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE UserRole SET Active = @IsActive WHERE UserRoleId = @UserRoleId", new { IsActive = isActive, UserRoleId = userRoleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int userRoleId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE UserRole SET Deleted = @IsDeleted WHERE UserRoleId = @UserRoleId", new { IsDeleted = isDeleted, UserRoleId = userRoleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
