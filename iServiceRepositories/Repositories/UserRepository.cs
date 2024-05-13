using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<User>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<User>("SELECT * FROM User");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<User>> GetUserByUserRoleIdAsync(int userRoleId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<User>(
                    "SELECT * FROM User WHERE UserRoleId = @UserRoleId", new { UserRoleId = userRoleId });
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<User>> GetUserByEstablishmentCategoryIdAsync(int establishmentCategoryId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<User>(
                    "SELECT U.UserId, U.UserRoleId, U.Email, U.Password, U.Name, U.CreationDate, U.LastLogin, U.LastUpdateDate FROM User U RIGHT JOIN UserProfile UP ON UP.UserId = U.UserId WHERE U.UserRoleId = 2 AND UP.EstablishmentCategoryId = @EstablishmentCategoryId",
                    new { EstablishmentCategoryId = establishmentCategoryId });
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM User WHERE UserId = @UserId", new { UserId = userId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM User WHERE Email = @Email", new { Email = email });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<bool> CheckUserAsync(string email)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                int count = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(*) FROM User WHERE Email = @Email", new { Email = email });
                return count > 0;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<User> InsertAsync(User userModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO User (UserRoleId, Email, Password, Name) VALUES (@UserRoleId, @Email, @Password, @Name); SELECT LAST_INSERT_Id();", userModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<User> UpdateAsync(User userUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE User SET UserRoleId = @UserRoleId, Email = @Email, Password = @Password, Name = @Name, LastLogin = @LastLogin, LastUpdateDate = NOW() WHERE UserId = @UserId", userUpdateModel);
                return await GetByIdAsync(userUpdateModel.UserId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<User> UpdateNameAsync(int userId, string name)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE User SET Name = @Name, LastUpdateDate = NOW() WHERE UserId = @UserId", new { UserId = userId, Name = name });
                return await GetByIdAsync(userId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                int affectedRows = await connection.ExecuteAsync(
                    "DELETE FROM User WHERE UserId = @UserId", new { UserId = userId });
                return affectedRows > 0;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
