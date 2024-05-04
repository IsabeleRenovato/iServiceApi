using Dapper;
using iServiceRepositories.Repositories.Models;
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
                return connection.Query<Service>("SELECT ServiceId, UserProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, ServiceImage, Active, Deleted, CreationDate, LastUpdateDate FROM Service").AsList();
            }
        }

        public Service GetById(int serviceId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Service>("SELECT ServiceId, UserProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, ServiceImage, Active, Deleted, CreationDate, LastUpdateDate FROM Service WHERE ServiceId = @ServiceId", new { ServiceId = serviceId });
            }
        }

        public Service Insert(ServiceInsert serviceModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Service (UserProfileId, ServiceCategoryId, Name, Description, Price, EstimatedDuration, ServiceImage) VALUES (@UserProfileId, @ServiceCategoryId, @Name, @Description, @Price, @EstimatedDuration, @ServiceImage); SELECT LAST_INSERT_Id();", serviceModel);
                return GetById(id);
            }
        }

        public Service Update(ServiceUpdate serviceUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Service SET UserProfileId = @UserProfileId, ServiceCategoryId = @ServiceCategoryId, Name = @Name, Description = @Description, Price = @Price, EstimatedDuration = @EstimatedDuration, ServiceImage = @ServiceImage, LastUpdateDate = NOW() WHERE ServiceId = @ServiceId", serviceUpdateModel);
                return GetById(serviceUpdateModel.ServiceId);
            }
        }

        public bool UpdateServiceImage(int id, string path)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("UPDATE Service SET ServiceImage = @ServiceImage, LastUpdateDate = NOW() WHERE ServiceId = @ServiceId", new { ServiceId = id, ServiceImage = path });
                return affectedRows > 0;
            }
        }

        public void SetActiveStatus(int serviceId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Service SET Active = @IsActive WHERE ServiceId = @ServiceId", new { IsActive = isActive, ServiceId = serviceId });
            }
        }

        public void SetDeletedStatus(int serviceId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Service SET Deleted = @IsDeleted WHERE ServiceId = @ServiceId", new { IsDeleted = isDeleted, ServiceId = serviceId });
            }
        }
    }
}
