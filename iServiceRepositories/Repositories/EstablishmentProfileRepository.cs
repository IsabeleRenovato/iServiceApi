using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class EstablishmentProfileRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public EstablishmentProfileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<EstablishmentProfile> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<EstablishmentProfile>("SELECT EstablishmentProfileId, UserId, CNPJ, CommercialName, EstablishmentCategoryId, AddressId, Description, CommercialPhone, CommercialEmail, Photo, CreationDate, LastUpdateDate FROM EstablishmentProfile").AsList();
            }
        }

        public EstablishmentProfile GetById(int establishmentProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<EstablishmentProfile>("SELECT EstablishmentProfileId, UserId, CNPJ, CommercialName, EstablishmentCategoryId, AddressId, Description, CommercialPhone, CommercialEmail, Photo, CreationDate, LastUpdateDate FROM EstablishmentProfile WHERE EstablishmentProfileId = @EstablishmentProfileId", new { EstablishmentProfileId = establishmentProfileId });
            }
        }

        public List<EstablishmentProfile> GetByEstablishmentCategoryId(int establishmentCategoryId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<EstablishmentProfile>("SELECT EstablishmentProfileId, UserId, CNPJ, CommercialName, EstablishmentCategoryId, AddressId, Description, CommercialPhone, CommercialEmail, Photo, CreationDate, LastUpdateDate FROM EstablishmentProfile WHERE EstablishmentCategoryId = @EstablishmentCategoryId", new { EstablishmentCategoryId = establishmentCategoryId }).AsList();
            }
        }

        public EstablishmentProfile GetByUserId(int userId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<EstablishmentProfile>("SELECT EstablishmentProfileId, UserId, CNPJ, CommercialName, EstablishmentCategoryId, AddressId, Description, CommercialPhone, CommercialEmail, Photo, CreationDate, LastUpdateDate FROM EstablishmentProfile WHERE UserId = @UserId", new { UserId = userId });
            }
        }

        public EstablishmentProfile Insert(EstablishmentProfileModel establishmentProfileModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO EstablishmentProfile (UserId, CNPJ, CommercialName, EstablishmentCategoryId, AddressId, Description, CommercialPhone, CommercialEmail, Photo) VALUES (@UserId, @CNPJ, @CommercialName, @EstablishmentCategoryId, @AddressId, @Description, @CommercialPhone, @CommercialEmail, @Photo); SELECT LAST_INSERT_Id();", establishmentProfileModel);
                return GetById(id);
            }
        }

        public EstablishmentProfile Update(EstablishmentProfile establishmentProfile)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE EstablishmentProfile SET UserId = @UserId, CNPJ = @CNPJ, CommercialName = @CommercialName, EstablishmentCategoryId = @EstablishmentCategoryId, AddressId = @AddressId, Description = @Description, CommercialPhone = @CommercialPhone, CommercialEmail = @CommercialEmail, Photo = @Photo, LastUpdateDate = NOW() WHERE EstablishmentProfileId = @EstablishmentProfileId", establishmentProfile);
                return GetById(establishmentProfile.EstablishmentProfileId);
            }
        }

        public bool UpdatePhoto(ImageModel photo)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int rowsAffected = connection.Execute("UPDATE EstablishmentProfile SET Photo = @Photo, LastUpdateDate = NOW() WHERE EstablishmentProfileId = @Id", photo);
                return rowsAffected > 0;
            }
        }

        public bool Delete(int establishmentProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM EstablishmentProfile WHERE EstablishmentProfileId = @EstablishmentProfileId", new { EstablishmentProfileId = establishmentProfileId });
                return affectedRows > 0;
            }
        }
    }
}
