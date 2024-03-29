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
                return connection.Query<Service>("SELECT ServiceId, EstablishmentProfileID, Title, Description, Price, Duration, CreationDate, LastUpdateDate FROM Service").AsList();
            }
        }

        public Service GetById(int serviceId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Service>("SELECT ServiceId, EstablishmentProfileID, Title, Description, Price, Duration, CreationDate, LastUpdateDate FROM Service WHERE ServiceId = @ServiceId", new { ServiceId = serviceId });
            }
        }

        public Service Insert(ServiceModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Service (EstablishmentProfileID, Title, Description, Price, Duration) VALUES (@EstablishmentProfileID, @Title, @Description, @Price, @Duration); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public Service Update(Service service)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Service SET EstablishmentProfileID = @EstablishmentProfileID, Title = @Title, Description = @Description, Price = @Price, Duration = @Duration, LastUpdateDate = NOW() WHERE ServiceId = @ServiceId", service);
                return GetById(service.ServiceId);
            }
        }

        public bool Delete(int serviceId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Service WHERE ServiceId = @ServiceId", new { ServiceId = serviceId });
                return affectedRows > 0;
            }
        }
    }
}
