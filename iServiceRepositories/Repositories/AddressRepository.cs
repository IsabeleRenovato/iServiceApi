using Dapper;
using iServiceRepositories.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace iServiceRepositories.Repositories
{
    public class AddressRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public AddressRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public Address Get(int? addressId)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"SELECT * FROM Address WHERE AddressId = @addressId";

                    return connection.QuerySingleOrDefault<Address>(query, new { addressId });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Address Insert(Address model)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"INSERT INTO Address (Street, Number, AdditionalInfo, City, State, Country, PostalCode)
                              VALUES (@Street, @Number, @AdditionalInfo, @City, @State, @Country, @PostalCode);
                              SELECT LAST_INSERT_ID();";

                    // Executando a inserção e capturando o último ID inserido
                    var id = connection.QuerySingle<int>(query, new
                    {
                        model.Street,
                        model.Number,
                        model.AdditionalInfo,
                        model.City,
                        model.State,
                        model.Country,
                        model.PostalCode
                    });

                    // Configurando o ID no modelo
                    model.AddressID = id;

                    // Retornando o modelo atualizado com o ID
                    return model;
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
