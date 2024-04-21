using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
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
                return connection.Query<Service>("SELECT ServiceId, EstablishmentProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, Photo, CreationDate, LastUpdateDate FROM Service").AsList();
            }
        }

        public Service GetById(int serviceId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Service>("SELECT ServiceId, EstablishmentProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, Photo, CreationDate, LastUpdateDate FROM Service WHERE ServiceId = @ServiceId", new { ServiceId = serviceId });
            }
        }

        public Service GetByServiceCategoryId(int serviceCategoryId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Service>("SELECT ServiceId, EstablishmentProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, Photo, CreationDate, LastUpdateDate FROM Service WHERE ServiceCategoryId = @ServiceCategoryId", new { ServiceCategoryId = serviceCategoryId });
            }
        }

        public Service Insert(ServiceModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Service (EstablishmentProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, Photo) VALUES (@EstablishmentProfileId, @ServiceCategoryId, @Name, @Description, @Price, @EstimatedDuration, @Photo); SELECT LAST_INSERT_Id();", model);
                return GetById(id);
            }
        }

        public Service Update(Service service)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Service SET EstablishmentProfileId = @EstablishmentProfileId, ServiceCategoryId = @ServiceCategoryId, Name = @Name, Description = @Description, Price = @Price, EstimatedDuration = @EstimatedDuration, Photo = @Photo, LastUpdateDate = NOW() WHERE ServiceId = @ServiceId", service);
                return GetById(service.ServiceId);
            }
        }

        public bool UpdatePhoto(ImageModel photo)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int rowsAffected = connection.Execute("UPDATE Service SET Photo = @Photo, LastUpdateDate = NOW() WHERE ServiceId = @Id", photo);
                return rowsAffected > 0;
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
