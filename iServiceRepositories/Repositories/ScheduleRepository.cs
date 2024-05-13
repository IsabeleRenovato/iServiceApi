using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class ScheduleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public ScheduleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<Schedule>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<Schedule>("SELECT * FROM Schedule");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Schedule> GetByIdAsync(int scheduleId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<Schedule>(
                    "SELECT * FROM Schedule WHERE ScheduleId = @ScheduleId", new { ScheduleId = scheduleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Schedule> GetByUserProfileIdAsync(int userProfileId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<Schedule>(
                    "SELECT * FROM Schedule WHERE UserProfileId = @UserProfileId AND Deleted = 0", new { UserProfileId = userProfileId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Schedule> InsertAsync(Schedule scheduleModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Schedule (UserProfileId, Days, Start, End, BreakStart, BreakEnd) VALUES (@UserProfileId, @Days, @Start, @End, @BreakStart, @BreakEnd); SELECT LAST_INSERT_Id();", scheduleModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Schedule> UpdateAsync(Schedule scheduleUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Schedule SET Days = @Days, Start = @Start, End = @End, BreakStart = @BreakStart, BreakEnd = @BreakEnd, LastUpdateDate = NOW() WHERE ScheduleId = @ScheduleId", scheduleUpdateModel);
                return await GetByIdAsync(scheduleUpdateModel.ScheduleId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int scheduleId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Schedule SET Active = @IsActive WHERE ScheduleId = @ScheduleId", new { IsActive = isActive, ScheduleId = scheduleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int scheduleId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Schedule SET Deleted = @IsDeleted WHERE ScheduleId = @ScheduleId", new { IsDeleted = isDeleted, ScheduleId = scheduleId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
