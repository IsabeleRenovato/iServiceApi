using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using Microsoft.Extensions.Configuration;

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

        public List<UserRole> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<UserRole>("SELECT UserRoleId, Role, CreationDate, LastUpdateDate FROM UserRole").AsList();
            }
        }

        public UserRole GetById(int userRoleId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<UserRole>("SELECT UserRoleId, Role, CreationDate, LastUpdateDate FROM UserRole WHERE UserRoleId = @UserRoleId", new { UserRoleId = userRoleId });
            }
        }

        public UserRole Insert(UserRoleModel userRoleModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO UserRole (Role) VALUES (@Role); SELECT LAST_INSERT_ID();", userRoleModel);
                return GetById(id);
            }
        }

        public UserRole Update(UserRole userRole)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE UserRole SET Role = @Role, LastUpdateDate = NOW() WHERE UserRoleId = @UserRoleId", userRole);
                return GetById(userRole.UserRoleId);
            }
        }

        public bool Delete(int userRoleId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM UserRole WHERE UserRoleId = @UserRoleId", new { UserRoleId = userRoleId });
                return affectedRows > 0;
            }
        }
    }

}
