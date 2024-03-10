using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using Microsoft.Extensions.Configuration;

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

        public List<User> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<User>("SELECT UserID, UserRoleID, Email, Password, Name, CreationDate, LastLogin, LastUpdateDate FROM User").AsList();
            }
        }

        public User GetById(int userId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<User>("SELECT UserID, UserRoleID, Email, Password, Name, CreationDate, LastLogin, LastUpdateDate FROM User WHERE UserID = @UserID", new { UserID = userId });
            }
        }

        public User GetByEmail(string email)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<User>("SELECT UserID, UserRoleID, Email, Password, Name, CreationDate, LastLogin, LastUpdateDate FROM User WHERE Email = @Email", new { Email = email });
            }
        }

        public User Insert(UserModel userModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO User (UserRoleID, Email, Password, Name) VALUES (@UserRoleID, @Email, @Password, @Name); SELECT LAST_INSERT_ID();", userModel);
                return GetById(id);
            }
        }

        public User Update(User user)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE User SET UserRoleID = @UserRoleID, Email = @Email, Password = @Password, Name = @Name, LastUpdateDate = NOW() WHERE UserID = @UserID", user);
                return GetById(user.UserID);
            }
        }

        public bool Delete(int userId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM User WHERE UserID = @UserID", new { UserID = userId });
                return affectedRows > 0;
            }
        }
    }
}
