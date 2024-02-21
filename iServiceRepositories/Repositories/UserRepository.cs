using Dapper;
using iServiceRepositories.Models;
using iServiceRepositories.Models.Auth;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace iServiceRepositories.Repositories
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<(User, UserRole)> GetAll()
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = "SELECT * FROM User U INNER JOIN UserRole UR ON U.UserRoleID = UR.UserRoleID;";
                    var result = connection.Query<User, UserRole, (User, UserRole)>(
                        query,
                        (user, role) =>
                        {
                            return (user, role);
                        },
                        splitOn: "UserRoleID"
                    ).ToList();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _connectionSingleton.CloseConnection();
            }
        }

        public (User User, UserRole UserRole) Get(string email, string password)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"SELECT 
                                    *
                                  FROM User U 
                                  INNER JOIN UserRole UR 
                                        ON U.UserRoleID = UR.UserRoleID";

                    var response = connection.Query<User, UserRole, (User User, UserRole UserRole)>(
                        query,
                        (user, role) =>
                        {
                            return (user, role);
                        },
                        splitOn: "UserRoleID"
                    ).ToList();

                    return response
                        .FirstOrDefault(x =>
                            x.User.Email.Equals(email, StringComparison.OrdinalIgnoreCase) // Considerando case-insensitive
                            && x.User.Password == password); // Certifique-se de que a senha está sendo tratada de forma segura
                }
            }
            catch (Exception ex)
            {
                // Aqui você pode adicionar algum log sobre a exceção
                throw;
            }
        }

        public bool Insert(PreRegister model)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"INSERT INTO User (UserRoleID, Email, Password, Name) 
                              VALUES (@UserRoleID, @Email, @Password, @Name)";

                    int rowsAffected = connection.Execute(query, new
                    {
                        model.UserRoleID,
                        model.Email,
                        model.Password,
                        model.Name
                    });

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Aqui você pode adicionar algum log sobre a exceção
                throw;
            }
        }

        public (User User, UserRole UserRole) GetUser(int userId)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @$"SELECT U.*, UR.* 
                               FROM User U 
                               INNER JOIN UserRole UR ON U.UserRoleID = UR.UserRoleID
                               WHERE U.UserId = @UserId";


                    var response = connection.Query<User, UserRole, (User User, UserRole UserRole)>(
                        query,
                        (user, role) =>
                        {
                            return (user, role);
                        },
                        param: new { UserId = userId },
                        splitOn: "UserRoleID"
                    ).FirstOrDefault();

                    return response;
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
