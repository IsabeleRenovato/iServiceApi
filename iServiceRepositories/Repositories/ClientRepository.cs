using Dapper;
using iServiceRepositories.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace iServiceRepositories.Repositories
{
    public class ClientRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public ClientRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public ClientProfile Get(int? userId)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"SELECT * FROM ClientProfile WHERE UserId = @userId";

                    return connection.QuerySingleOrDefault<ClientProfile>(query, new { userId });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ClientProfile Insert(ClientProfile model)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var insertQuery = @"INSERT INTO ClientProfile (UserID, CPF, DateOfBirth, Phone, AddressID, ProfilePicture) 
                                    VALUES (@UserID, @CPF, @DateOfBirth, @Phone, @AddressID, @ProfilePicture);
                                    SELECT * FROM ClientProfile WHERE ClientProfileId = LAST_INSERT_ID();";

                    model = connection.QuerySingleOrDefault<ClientProfile>(insertQuery, new
                    {
                        model.UserID,
                        model.CPF,
                        model.DateOfBirth,
                        model.Phone,
                        model.AddressID,
                        model.ProfilePicture
                    });

                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
