using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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
                return connection.Query<EstablishmentCategory>("SELECT EstablishmentCategoryID, Name, CreationDate, LastUpdateDate FROM EstablishmentCategory").AsList();
            }
        }

        public EstablishmentCategory GetById(int establishmentCategoryId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<EstablishmentCategory>("SELECT EstablishmentCategoryID, Name, CreationDate, LastUpdateDate FROM EstablishmentCategory WHERE EstablishmentCategoryID = @EstablishmentCategoryID", new { EstablishmentCategoryID = establishmentCategoryId });
            }
        }

        public EstablishmentCategory Insert(EstablishmentCategoryModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO EstablishmentCategory (Name) VALUES (@Name); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public EstablishmentCategory Update(EstablishmentCategory category)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE EstablishmentCategory SET Name = @Name, LastUpdateDate = NOW() WHERE EstablishmentCategoryID = @EstablishmentCategoryID", category);
                return GetById(category.EstablishmentCategoryID);
            }
        }

        public bool Delete(int establishmentCategoryId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM EstablishmentCategory WHERE EstablishmentCategoryID = @EstablishmentCategoryID", new { EstablishmentCategoryID = establishmentCategoryId });
                return affectedRows > 0;
            }
        }
    }
}
