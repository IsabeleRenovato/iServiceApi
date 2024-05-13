using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class SpecialScheduleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public SpecialScheduleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<SpecialSchedule>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<SpecialSchedule>("SELECT * FROM SpecialSchedule");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<SpecialSchedule> GetByIdAsync(int specialScheduleId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<SpecialSchedule>(
                    "SELECT * FROM SpecialSchedule WHERE SpecialScheduleId = @SpecialScheduleId", new { SpecialScheduleId = specialScheduleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<SpecialSchedule>> GetByUserProfileIdAsync(int userProfileId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<SpecialSchedule>(
                    "SELECT * FROM SpecialSchedule WHERE EstablishmentUserProfileId = @EstablishmentUserProfileId AND Deleted = 0", new { EstablishmentUserProfileId = userProfileId });
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<SpecialSchedule> InsertAsync(SpecialSchedule specialScheduleModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO SpecialSchedule (EstablishmentUserProfileId, Date, Start, End, BreakStart, BreakEnd) VALUES (@EstablishmentUserProfileId, @Date, @Start, @End, @BreakStart, @BreakEnd); SELECT LAST_INSERT_Id();", specialScheduleModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<SpecialSchedule> UpdateAsync(SpecialSchedule specialScheduleUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE SpecialSchedule SET Date = @Date, Start = @Start, End = @End, BreakStart = @BreakStart, BreakEnd = @BreakEnd, LastUpdateDate = NOW() WHERE SpecialScheduleId = @SpecialScheduleId", specialScheduleUpdateModel);
                return await GetByIdAsync(specialScheduleUpdateModel.SpecialScheduleId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int specialScheduleId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE SpecialSchedule SET Active = @IsActive WHERE SpecialScheduleId = @SpecialScheduleId", new { IsActive = isActive, SpecialScheduleId = specialScheduleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int specialScheduleId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE SpecialSchedule SET Deleted = @IsDeleted WHERE SpecialScheduleId = @SpecialScheduleId", new { IsDeleted = isDeleted, SpecialScheduleId = specialScheduleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
