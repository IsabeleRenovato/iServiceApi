using Dapper;
using iServiceRepositories.Repositories.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class AppointmentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public AppointmentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public async Task<List<Appointment>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<Appointment>("SELECT * FROM Appointment");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Appointment> GetByIdAsync(int appointmentId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<Appointment>(
                    "SELECT * FROM Appointment WHERE AppointmentId = @AppointmentId", new { AppointmentId = appointmentId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Appointment> InsertAsync(Appointment appointmentModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Appointment (ServiceId, ClientUserProfileId, EstablishmentUserProfileId, AppointmentStatusId, Start, End) VALUES (@ServiceId, @ClientUserProfileId, @EstablishmentUserProfileId, @AppointmentStatusId, @Start, @End); SELECT LAST_INSERT_Id();", appointmentModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Appointment> UpdateAsync(Appointment appointmentUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Appointment SET AppointmentStatusId = @AppointmentStatusId, Start = @Start, End = @End, LastUpdateDate = NOW() WHERE AppointmentId = @AppointmentId", appointmentUpdateModel);
                return await GetByIdAsync(appointmentUpdateModel.AppointmentId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int appointmentId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Appointment SET Active = @IsActive WHERE AppointmentId = @AppointmentId", new { IsActive = isActive, AppointmentId = appointmentId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int appointmentId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Appointment SET Deleted = @IsDeleted WHERE AppointmentId = @AppointmentId", new { IsDeleted = isDeleted, AppointmentId = appointmentId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
