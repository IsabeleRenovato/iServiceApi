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

        public async Task<List<EstablishmentCategory>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<EstablishmentCategory>("SELECT * FROM EstablishmentCategory WHERE Active = 1 AND Deleted = 0");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<EstablishmentCategory> GetByIdAsync(int establishmentCategoryId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<EstablishmentCategory>(
                    "SELECT * FROM EstablishmentCategory WHERE EstablishmentCategoryId = @EstablishmentCategoryId AND Active = 1 AND Deleted = 0", new { EstablishmentCategoryId = establishmentCategoryId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<EstablishmentCategory> InsertAsync(EstablishmentCategory establishmentCategoryModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO EstablishmentCategory (Name) VALUES (@Name); SELECT LAST_INSERT_Id();", establishmentCategoryModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<EstablishmentCategory> UpdateAsync(EstablishmentCategory establishmentCategoryUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE EstablishmentCategory SET Name = @Name, LastUpdateDate = NOW() WHERE EstablishmentCategoryId = @EstablishmentCategoryId", establishmentCategoryUpdateModel);
                return await GetByIdAsync(establishmentCategoryUpdateModel.EstablishmentCategoryId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int establishmentCategoryId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE EstablishmentCategory SET Active = @IsActive WHERE EstablishmentCategoryId = @EstablishmentCategoryId", new { IsActive = isActive, EstablishmentCategoryId = establishmentCategoryId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int establishmentCategoryId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE EstablishmentCategory SET Deleted = @IsDeleted WHERE EstablishmentCategoryId = @EstablishmentCategoryId", new { IsDeleted = isDeleted, EstablishmentCategoryId = establishmentCategoryId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
