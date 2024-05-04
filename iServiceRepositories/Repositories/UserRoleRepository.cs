using Dapper;
using iServiceRepositories.Repositories.Models;
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
                return connection.Query<UserRole>("SELECT UserRoleId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM UserRole").AsList();
            }
        }

        public UserRole GetById(int userRoleId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<UserRole>("SELECT UserRoleId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM UserRole WHERE UserRoleId = @UserRoleId", new { UserRoleId = userRoleId });
            }
        }

        public UserRole Insert(UserRoleInsert userRoleModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO UserRole (Name) VALUES (@Name); SELECT LAST_INSERT_Id();", userRoleModel);
                return GetById(id);
            }
        }

        public UserRole Update(UserRoleUpdate userRoleUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE UserRole SET Name = @Name, LastUpdateDate = NOW() WHERE UserRoleId = @UserRoleId", userRoleUpdateModel);
                return GetById(userRoleUpdateModel.UserRoleId);
            }
        }

        public void SetActiveStatus(int userRoleId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE UserRole SET Active = @IsActive WHERE UserRoleId = @UserRoleId", new { IsActive = isActive, UserRoleId = userRoleId });
            }
        }

        public void SetDeletedStatus(int userRoleId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE UserRole SET Deleted = @IsDeleted WHERE UserRoleId = @UserRoleId", new { IsDeleted = isDeleted, UserRoleId = userRoleId });
            }
        }
    }
}
