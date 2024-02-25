using Dapper;
using iServiceRepositories.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

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

        public UserRole Get(int userRoleId)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"SELECT * FROM UserRole WHERE UserRoleId = @userRoleId";

                    return connection.QuerySingleOrDefault<UserRole>(query, new { userRoleId });
                }
            }
            catch (Exception ex)
            {
                // Aqui você pode adicionar algum log sobre a exceção
                throw;
            }
        }
    }
}
