using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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
                return connection.Query<ServiceCategory>("SELECT ServiceCategoryID, Name, CreationDate, LastUpdateDate FROM ServiceCategory").AsList();
            }
        }

        public ServiceCategory GetById(int serviceCategoryID)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<ServiceCategory>("SELECT ServiceCategoryID, Name, CreationDate, LastUpdateDate FROM ServiceCategory WHERE ServiceCategoryID = @ServiceCategoryID", new { ServiceCategoryID = serviceCategoryID });
            }
        }

        public ServiceCategory Insert(ServiceCategoryModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO ServiceCategory (Name) VALUES (@Name); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public ServiceCategory Update(ServiceCategory serviceCategory)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE ServiceCategory SET Name = @Name, LastUpdateDate = NOW() WHERE ServiceCategoryID = @ServiceCategoryID", serviceCategory);
                return GetById(serviceCategory.ServiceCategoryId);
            }
        }

        public bool Delete(int serviceCategoryID)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM ServiceCategory WHERE ServiceCategoryID = @ServiceCategoryID", new { ServiceCategoryID = serviceCategoryID });
                return affectedRows > 0;
            }
        }
    }
}
