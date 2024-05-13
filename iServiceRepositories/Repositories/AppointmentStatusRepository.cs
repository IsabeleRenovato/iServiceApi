using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class AppointmentStatusRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public AppointmentStatusRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<AppointmentStatus>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<AppointmentStatus>("SELECT * FROM AppointmentStatus");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<AppointmentStatus> GetByIdAsync(int appointmentStatusId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<AppointmentStatus>(
                    "SELECT * FROM AppointmentStatus WHERE AppointmentStatusId = @AppointmentStatusId", new { AppointmentStatusId = appointmentStatusId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<AppointmentStatus> InsertAsync(AppointmentStatus appointmentStatusModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO AppointmentStatus (Name) VALUES (@Name); SELECT LAST_INSERT_Id();", appointmentStatusModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<AppointmentStatus> UpdateAsync(AppointmentStatus appointmentStatus)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE AppointmentStatus SET Name = @Name, LastUpdateDate = NOW() WHERE AppointmentStatusId = @AppointmentStatusId", appointmentStatus);
                return await GetByIdAsync(appointmentStatus.AppointmentStatusId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int appointmentStatusId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE AppointmentStatus SET Active = @IsActive WHERE AppointmentStatusId = @AppointmentStatusId", new { IsActive = isActive, AppointmentStatusId = appointmentStatusId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int appointmentStatusId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE AppointmentStatus SET Deleted = @IsDeleted WHERE AppointmentStatusId = @AppointmentStatusId", new { IsDeleted = isDeleted, AppointmentStatusId = appointmentStatusId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
