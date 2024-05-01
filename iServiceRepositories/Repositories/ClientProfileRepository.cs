using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
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
                return connection.Query<ClientProfile>("SELECT ClientProfileId, UserId, CPF, DateOfBirth, Phone, AddressId, Photo, CreationDate, LastUpdateDate FROM ClientProfile").AsList();
            }
        }

        public ClientProfile GetById(int clientProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<ClientProfile>("SELECT ClientProfileId, UserId, CPF, DateOfBirth, Phone, AddressId, Photo, CreationDate, LastUpdateDate FROM ClientProfile WHERE ClientProfileId = @ClientProfileId", new { ClientProfileId = clientProfileId });
            }
        }

        public ClientProfile GetByUserId(int userId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<ClientProfile>("SELECT ClientProfileId, UserId, CPF, DateOfBirth, Phone, AddressId, Photo, CreationDate, LastUpdateDate FROM ClientProfile WHERE UserId = @UserId", new { UserId = userId });
            }
        }

        public ClientProfile Insert(ClientProfileModel clientProfileModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO ClientProfile (UserId, CPF, DateOfBirth, Phone, AddressId, Photo) VALUES (@UserId, @CPF, @DateOfBirth, @Phone, @AddressId, @Photo); SELECT LAST_INSERT_Id();", clientProfileModel);
                return GetById(id);
            }
        }

        public ClientProfile Update(ClientProfile clientProfile)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE ClientProfile SET UserId = @UserId, CPF = @CPF, DateOfBirth = @DateOfBirth, Phone = @Phone, AddressId = @AddressId, Photo = @Photo, LastUpdateDate = NOW() WHERE ClientProfileId = @ClientProfileId", clientProfile);
                return GetById(clientProfile.ClientProfileId);
            }
        }

        public bool UpdatePhoto(int id, string path)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int rowsAffected = connection.Execute("UPDATE ClientProfile SET Photo = @Photo, LastUpdateDate = NOW() WHERE UserId = @Id", new { Id = id, Photo = path });
                return rowsAffected > 0;
            }
        }

        public bool Delete(int clientProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM ClientProfile WHERE ClientProfileId = @ClientProfileId", new { ClientProfileId = clientProfileId });
                return affectedRows > 0;
            }
        }
    }

}
