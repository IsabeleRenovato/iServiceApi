using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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
                return connection.Query<EstablishmentProfile>("SELECT EstablishmentProfileID, UserID, CNPJ, CommercialName, EstablishmentCategoryID, AddressID, Description, CommercialPhone, CommercialEmail, Photo, CreationDate, LastUpdateDate FROM EstablishmentProfile").AsList();
            }
        }

        public EstablishmentProfile GetById(int establishmentProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<EstablishmentProfile>("SELECT EstablishmentProfileID, UserID, CNPJ, CommercialName, EstablishmentCategoryID, AddressID, Description, CommercialPhone, CommercialEmail, Photo, CreationDate, LastUpdateDate FROM EstablishmentProfile WHERE EstablishmentProfileID = @EstablishmentProfileID", new { EstablishmentProfileID = establishmentProfileId });
            }
        }

        public EstablishmentProfile GetByUserId(int userId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<EstablishmentProfile>("SELECT EstablishmentProfileID, UserID, CNPJ, CommercialName, EstablishmentCategoryID, AddressID, Description, CommercialPhone, CommercialEmail, Photo, CreationDate, LastUpdateDate FROM EstablishmentProfile WHERE UserID = @UserID", new { UserID = userId });
            }
        }

        public EstablishmentProfile Insert(EstablishmentProfileModel establishmentProfileModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO EstablishmentProfile (UserID, CNPJ, CommercialName, EstablishmentCategoryID, AddressID, Description, CommercialPhone, CommercialEmail, Photo) VALUES (@UserID, @CNPJ, @CommercialName, @EstablishmentCategoryID, @AddressID, @Description, @CommercialPhone, @CommercialEmail, @Photo); SELECT LAST_INSERT_ID();", establishmentProfileModel);
                return GetById(id);
            }
        }

        public EstablishmentProfile Update(EstablishmentProfile establishmentProfile)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE EstablishmentProfile SET UserID = @UserID, CNPJ = @CNPJ, CommercialName = @CommercialName, EstablishmentCategoryID = @EstablishmentCategoryID, AddressID = @AddressID, Description = @Description, CommercialPhone = @CommercialPhone, CommercialEmail = @CommercialEmail, Photo = @Photo, LastUpdateDate = NOW() WHERE EstablishmentProfileID = @EstablishmentProfileID", establishmentProfile);
                return GetById(establishmentProfile.EstablishmentProfileID);
            }
        }

        public bool Delete(int establishmentProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM EstablishmentProfile WHERE EstablishmentProfileID = @EstablishmentProfileID", new { EstablishmentProfileID = establishmentProfileId });
                return affectedRows > 0;
            }
        }
    }
}
