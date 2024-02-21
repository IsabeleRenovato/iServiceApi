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

        public ClientProfile Insert(ClientProfile model)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var insertQuery = @"INSERT INTO ClientProfile (UserID, CPF, DateOfBirth, Phone, AddressID, ProfilePicture) 
                                    VALUES (@UserID, @CPF, @DateOfBirth, @Phone, @AddressID, @ProfilePicture);
                                    SELECT LAST_INSERT_ID();";

                    // Executando a inserção e capturando o último ID inserido
                    var id = connection.QuerySingle<int>(insertQuery, new
                    {
                        model.UserID,
                        model.CPF,
                        model.DateOfBirth,
                        model.Phone,
                        model.AddressID,
                        model.ProfilePicture
                    });

                    // Configurando o ID no modelo
                    model.ClientProfileID = id;

                    // Opcionalmente, você pode querer atualizar o CreationDate no modelo, se ele for gerado automaticamente pelo banco de dados
                    // Isso requer uma consulta adicional ou ajustar a estratégia para capturar esse valor

                    return model; // Retornando o modelo atualizado com o ID
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
