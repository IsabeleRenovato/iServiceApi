using Dapper;
using iServiceRepositories.Models;
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

        public User Login(string email, string password)
        {
            try
            {
                using (var connection = _connectionSingleton.GetConnection())
                {
                    var query = @"SELECT * FROM User WHERE Email = @Email;";

                    var user = connection.QuerySingleOrDefault<User>(query, new { Email = email });

                    if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        return user;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar fazer login.", ex);
            }
        }

        public User Insert(User model)
        {
            try
            {
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @"INSERT INTO User (UserRoleID, Email, Password, Name) 
                          VALUES (@UserRoleID, @Email, @Password, @Name);
                          SELECT * FROM User WHERE UserID = LAST_INSERT_ID();";

                    model = connection.QuerySingle<User>(query, new
                    {
                        model.UserRoleID,
                        model.Email,
                        model.Password,
                        model.Name
                    });

                    return model;
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

        public bool CheckUser(string email)
        {
            try
            {
                using (MySqlConnection connection = _connectionSingleton.GetConnection())
                {
                    var query = @$"SELECT 
                                       COUNT(*)
                                   FROM User
                                   WHERE Email = @email;";

                    return connection.QuerySingle<bool>(query, new { email });
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
