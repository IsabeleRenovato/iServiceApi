using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class ServiceRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public ServiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<Service> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Service>("SELECT ServiceID, EstablishmentProfileID, ServiceCategoryID, Name, Description, Price, EstimatedDuration, Photo, CreationDate, LastUpdateDate FROM Service").AsList();
            }
        }

        public Service GetById(int serviceID)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Service>("SELECT ServiceID, EstablishmentProfileID, ServiceCategoryID, Name, Description, Price, EstimatedDuration, Photo, CreationDate, LastUpdateDate FROM Service WHERE ServiceID = @ServiceID", new { ServiceID = serviceID });
            }
        }

        public Service Insert(ServiceModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Service (EstablishmentProfileID, ServiceCategoryID, Name, Description, Price, EstimatedDuration, Photo) VALUES (@EstablishmentProfileID, @ServiceCategoryID, @Name, @Description, @Price, @EstimatedDuration, @Photo); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public Service Update(Service service)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Service SET EstablishmentProfileID = @EstablishmentProfileID, ServiceCategoryID = @ServiceCategoryID, Name = @Name, Description = @Description, Price = @Price, EstimatedDuration = @EstimatedDuration, Photo = @Photo, LastUpdateDate = NOW() WHERE ServiceID = @ServiceID", service);
                return GetById(service.ServiceID);
            }
        }

        public bool Delete(int serviceID)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Service WHERE ServiceID = @ServiceID", new { ServiceID = serviceID });
                return affectedRows > 0;
            }
        }
    }
}
