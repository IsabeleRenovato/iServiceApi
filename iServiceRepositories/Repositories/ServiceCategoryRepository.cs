using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class ServiceCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public ServiceCategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<ServiceCategory> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<ServiceCategory>("SELECT ServiceCategoryId, UserProfileId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM ServiceCategory WHERE Deleted = 0").AsList();
            }
        }

        public ServiceCategory GetById(int serviceCategoryId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<ServiceCategory>("SELECT ServiceCategoryId, UserProfileId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM ServiceCategory WHERE ServiceCategoryId = @ServiceCategoryId AND Deleted = 0", new { ServiceCategoryId = serviceCategoryId });
            }
        }

        public List<ServiceCategory> GetByUserProfileId(int userProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<ServiceCategory>("SELECT ServiceCategoryId, UserProfileId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM ServiceCategory WHERE UserProfileId = @UserProfileId AND Deleted = 0", new { UserProfileId = userProfileId }).AsList();
            }
        }

        public ServiceCategory GetByFilter(int userProfileId, int serviceCategoryId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<ServiceCategory>("SELECT ServiceCategoryId, UserProfileId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM ServiceCategory WHERE ServiceCategoryId = @ServiceCategoryId AND UserProfileId = @UserProfileId", new { ServiceCategoryId = serviceCategoryId, UserProfileId = userProfileId });
            }
        }

        public ServiceCategory Insert(ServiceCategoryInsert serviceCategoryModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO ServiceCategory (UserProfileId, Name) VALUES (@UserProfileId, @Name); SELECT LAST_INSERT_Id();", serviceCategoryModel);
                return GetById(id);
            }
        }

        public ServiceCategory Update(ServiceCategoryUpdate serviceCategoryUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE ServiceCategory SET Name = @Name, LastUpdateDate = NOW() WHERE ServiceCategoryId = @ServiceCategoryId", serviceCategoryUpdateModel);
                return GetById(serviceCategoryUpdateModel.ServiceCategoryId);
            }
        }

        public void SetActiveStatus(int serviceCategoryId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE ServiceCategory SET Active = @IsActive WHERE ServiceCategoryId = @ServiceCategoryId", new { IsActive = isActive, ServiceCategoryId = serviceCategoryId });
            }
        }

        public void SetDeletedStatus(int serviceCategoryId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE ServiceCategory SET Deleted = @IsDeleted WHERE ServiceCategoryId = @ServiceCategoryId", new { IsDeleted = isDeleted, ServiceCategoryId = serviceCategoryId });
            }
        }
    }
}
