using Dapper;
using iServiceRepositories.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace iServiceRepositories.Repositories
{
    public class EstablishmentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public EstablishmentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public EstablishmentProfile Get(int? userId)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"SELECT * FROM EstablishmentProfile WHERE UserId = @userId";

                    return connection.QuerySingleOrDefault<EstablishmentProfile>(query, new { userId });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public EstablishmentProfile Insert(EstablishmentProfile model)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"INSERT INTO EstablishmentProfile (UserID, CNPJ, CommercialName, AddressID, Description, CommercialPhone, CommercialEmail, Logo) 
                              VALUES (@UserID, @CNPJ, @CommercialName, @AddressID, @Description, @CommercialPhone, @CommercialEmail, @Logo);
                              SELECT * FROM EstablishmentProfile WHERE EstablishmentProfileId = LAST_INSERT_ID();";

                    model = connection.QuerySingle<EstablishmentProfile>(query, new
                    {
                        model.UserID,
                        model.CNPJ,
                        model.CommercialName,
                        model.AddressID,
                        model.Description,
                        model.CommercialPhone,
                        model.CommercialEmail,
                        model.Logo
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
