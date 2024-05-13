using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class UserProfileRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public UserProfileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<UserProfile>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<UserProfile>("SELECT * FROM UserProfile");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<UserProfile> GetByIdAsync(int userProfileId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<UserProfile>(
                    "SELECT * FROM UserProfile WHERE UserProfileId = @UserProfileId", new { UserProfileId = userProfileId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<UserProfile> GetByUserIdAsync(int userId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<UserProfile>(
                    "SELECT * FROM UserProfile WHERE UserId = @UserId", new { UserId = userId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<UserProfile> InsertAsync(UserProfile userProfileModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO UserProfile (UserId, EstablishmentCategoryId, AddressId, Document, DateOfBirth, Phone, CommercialName, CommercialPhone, CommercialEmail, Description, ProfileImage) VALUES (@UserId, @EstablishmentCategoryId, @AddressId, @Document, @DateOfBirth, @Phone, @CommercialName, @CommercialPhone, @CommercialEmail, @Description, @ProfileImage); SELECT LAST_INSERT_Id();", userProfileModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<UserProfile> UpdateAsync(UserProfile userProfile)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE UserProfile SET EstablishmentCategoryId = @EstablishmentCategoryId, AddressId = @AddressId, Document = @Document, DateOfBirth = @DateOfBirth, Phone = @Phone, CommercialName = @CommercialName, CommercialPhone = @CommercialPhone, CommercialEmail = @CommercialEmail, Description = @Description, ProfileImage = @ProfileImage, LastUpdateDate = NOW() WHERE UserProfileId = @UserProfileId", userProfile);
                return await GetByIdAsync(userProfile.UserProfileId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAddressAsync(int userProfileId, int addressId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                int affectedRows = await connection.ExecuteAsync("UPDATE UserProfile SET AddressId = @AddressId WHERE UserProfileId = @UserProfileId", new { UserProfileId = userProfileId, AddressId = addressId });
                return affectedRows > 0;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateProfileImageAsync(int id, string path)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                int affectedRows = await connection.ExecuteAsync("UPDATE UserProfile SET ProfileImage = @ProfileImage, LastUpdateDate = NOW() WHERE UserProfileId = @UserProfileId", new { UserProfileId = id, ProfileImage = path });
                return affectedRows > 0;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<bool> DeleteAsync(int userProfileId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                int affectedRows = await connection.ExecuteAsync("DELETE FROM UserProfile WHERE UserProfileId = @UserProfileId", new { UserProfileId = userProfileId });
                return affectedRows > 0;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
