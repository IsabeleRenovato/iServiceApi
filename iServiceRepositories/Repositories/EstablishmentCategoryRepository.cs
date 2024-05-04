using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class EstablishmentCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public EstablishmentCategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<EstablishmentCategory> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<EstablishmentCategory>("SELECT EstablishmentCategoryId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM EstablishmentCategory").AsList();
            }
        }

        public EstablishmentCategory GetById(int establishmentCategoryId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<EstablishmentCategory>("SELECT EstablishmentCategoryId, Name, Active, Deleted, CreationDate, LastUpdateDate FROM EstablishmentCategory WHERE EstablishmentCategoryId = @EstablishmentCategoryId", new { EstablishmentCategoryId = establishmentCategoryId });
            }
        }

        public EstablishmentCategory Insert(EstablishmentCategoryInsert establishmentCategoryModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO EstablishmentCategory (Name) VALUES (@Name); SELECT LAST_INSERT_Id();", establishmentCategoryModel);
                return GetById(id);
            }
        }

        public EstablishmentCategory Update(EstablishmentCategoryUpdate establishmentCategoryUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE EstablishmentCategory SET Name = @Name, LastUpdateDate = NOW() WHERE EstablishmentCategoryId = @EstablishmentCategoryId", establishmentCategoryUpdateModel);
                return GetById(establishmentCategoryUpdateModel.EstablishmentCategoryId);
            }
        }

        public void SetActiveStatus(int establishmentCategoryId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE EstablishmentCategory SET Active = @IsActive WHERE EstablishmentCategoryId = @EstablishmentCategoryId", new { IsActive = isActive, EstablishmentCategoryId = establishmentCategoryId });
            }
        }

        public void SetDeletedStatus(int establishmentCategoryId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE EstablishmentCategory SET Deleted = @IsDeleted WHERE EstablishmentCategoryId = @EstablishmentCategoryId", new { IsDeleted = isDeleted, EstablishmentCategoryId = establishmentCategoryId });
            }
        }
    }
}
