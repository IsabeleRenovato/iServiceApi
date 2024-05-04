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

        public List<UserProfile> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<UserProfile>("SELECT UserProfileId, UserId, EstablishmentCategoryId, AddressId, Document, DateOfBirth, Phone, CommercialName, CommercialPhone, CommercialEmail, Description, ProfileImage, CreationDate, LastUpdateDate FROM UserProfile").AsList();
            }
        }

        public UserProfile GetById(int userProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<UserProfile>("SELECT UserProfileId, UserId, EstablishmentCategoryId, AddressId, Document, DateOfBirth, Phone, CommercialName, CommercialPhone, CommercialEmail, Description, ProfileImage, CreationDate, LastUpdateDate FROM UserProfile WHERE UserProfileId = @UserProfileId", new { UserProfileId = userProfileId });
            }
        }

        public UserProfile GetByUserId(int userId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<UserProfile>("SELECT UserProfileId, UserId, EstablishmentCategoryId, AddressId, Document, DateOfBirth, Phone, CommercialName, CommercialPhone, CommercialEmail, Description, ProfileImage, CreationDate, LastUpdateDate FROM UserProfile WHERE UserId = @UserId", new { UserId = userId });
            }
        }

        public UserProfile Insert(UserProfileInsert userProfileModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO UserProfile (UserId, EstablishmentCategoryId, AddressId, Document, DateOfBirth, Phone, CommercialName, CommercialPhone, CommercialEmail, Description, ProfileImage) VALUES (@UserId, @EstablishmentCategoryId, @AddressId, @Document, @DateOfBirth, @Phone, @CommercialName, @CommercialPhone, @CommercialEmail, @Description, @ProfileImage); SELECT LAST_INSERT_Id();", userProfileModel);
                return GetById(id);
            }
        }

        public UserProfile Update(UserProfileUpdate userProfile)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE UserProfile SET EstablishmentCategoryId = @EstablishmentCategoryId, AddressId = @AddressId, Document = @Document, DateOfBirth = @DateOfBirth, Phone = @Phone, CommercialName = @CommercialName, CommercialPhone = @CommercialPhone, CommercialEmail = @CommercialEmail, Description = @Description, ProfileImage = @ProfileImage, LastUpdateDate = NOW() WHERE UserProfileId = @UserProfileId", userProfile);
                return GetById(userProfile.UserProfileId);
            }
        }

        public bool UpdateAddress(int userProfileId, int addressId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("UPDATE UserProfile SET AddressId = @AddressId WHERE UserProfileId = @UserProfileId", new { UserProfileId = userProfileId, AddressId = addressId });
                return affectedRows > 0;
            }
        }

        public bool UpdateProfileImage(int id, string path)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("UPDATE UserProfile SET ProfileImage = @ProfileImage, LastUpdateDate = NOW() WHERE UserProfileId = @UserProfileId", new { UserProfileId = id, ProfileImage = path });
                return affectedRows > 0;
            }
        }

        public bool Delete(int userProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM UserProfile WHERE UserProfileId = @UserProfileId", new { UserProfileId = userProfileId });
                return affectedRows > 0;
            }
        }
    }
}
