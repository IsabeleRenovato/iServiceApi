using Dapper;
using iServiceRepositories.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public EstablishmentProfile Insert(EstablishmentProfile model)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"INSERT INTO EstablishmentProfile (UserID, CNPJ, CommercialName, AddressID, Description, CommercialPhone, CommercialEmail, Logo) 
                              VALUES (@UserID, @CNPJ, @CommercialName, @AddressID, @Description, @CommercialPhone, @CommercialEmail, @Logo);
                              SELECT LAST_INSERT_ID();";

                    // Executando a inserção e capturando o último ID inserido
                    var id = connection.QuerySingle<int>(query, new
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

                    // Configurando o ID no modelo
                    model.EstablishmentProfileID = id;

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
