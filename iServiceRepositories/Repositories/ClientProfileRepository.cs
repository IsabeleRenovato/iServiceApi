using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class ClientProfileRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public ClientProfileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<ClientProfile> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<ClientProfile>("SELECT ClientProfileID, UserID, CPF, DateOfBirth, Phone, AddressID, Photo, CreationDate, LastUpdateDate FROM ClientProfile").AsList();
            }
        }

        public ClientProfile GetById(int clientProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<ClientProfile>("SELECT ClientProfileID, UserID, CPF, DateOfBirth, Phone, AddressID, Photo, CreationDate, LastUpdateDate FROM ClientProfile WHERE ClientProfileID = @ClientProfileID", new { ClientProfileID = clientProfileId });
            }
        }

        public ClientProfile GetByUserId(int userId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<ClientProfile>("SELECT ClientProfileID, UserID, CPF, DateOfBirth, Phone, AddressID, Photo, CreationDate, LastUpdateDate FROM ClientProfile WHERE UserID = @UserID", new { UserID = userId });
            }
        }

        public ClientProfile Insert(ClientProfileModel clientProfileModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO ClientProfile (UserID, CPF, DateOfBirth, Phone, AddressID, Photo) VALUES (@UserID, @CPF, @DateOfBirth, @Phone, @AddressID, @Photo); SELECT LAST_INSERT_ID();", clientProfileModel);
                return GetById(id);
            }
        }

        public ClientProfile Update(ClientProfile clientProfile)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE ClientProfile SET UserID = @UserID, CPF = @CPF, DateOfBirth = @DateOfBirth, Phone = @Phone, AddressID = @AddressID, Photo = @Photo, LastUpdateDate = NOW() WHERE ClientProfileID = @ClientProfileID", clientProfile);
                return GetById(clientProfile.ClientProfileID);
            }
        }

        public bool Delete(int clientProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM ClientProfile WHERE ClientProfileID = @ClientProfileID", new { ClientProfileID = clientProfileId });
                return affectedRows > 0;
            }
        }
    }

}
