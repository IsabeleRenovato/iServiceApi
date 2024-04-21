using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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

        public List<Schedule> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Schedule>("SELECT ScheduleId, EstablishmentProfileId, Days, Start, End, BreakStart, BreakEnd, CreationDate, LastUpdateDate FROM Schedule").AsList();
            }
        }

        public Schedule GetById(int scheduleId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Schedule>("SELECT ScheduleId, EstablishmentProfileId, Days, Start, End, BreakStart, BreakEnd, CreationDate, LastUpdateDate FROM Schedule WHERE ScheduleId = @ScheduleId", new { ScheduleId = scheduleId });
            }
        }

        public Schedule GetByEstablishmentProfileId(int establishmentProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Schedule>("SELECT ScheduleId, EstablishmentProfileId, Days, Start, End, BreakStart, BreakEnd, CreationDate, LastUpdateDate FROM Schedule WHERE EstablishmentProfileId = @EstablishmentProfileId", new { EstablishmentProfileId = establishmentProfileId });
            }
        }

        public Schedule Insert(ScheduleModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Schedule (EstablishmentProfileId, Days, Start, End, BreakStart, BreakEnd) VALUES (@EstablishmentProfileId, @Days, @Start, @End, @BreakStart, @BreakEnd); SELECT LAST_INSERT_Id();", model);
                return GetById(id);
            }
        }

        public Schedule Update(Schedule schedule)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Schedule SET EstablishmentProfileId = @EstablishmentProfileId, Days = @Days, Start = @Start, End = @End, BreakStart = @BreakStart, BreakEnd = @BreakEnd, LastUpdateDate = NOW() WHERE ScheduleId = @ScheduleId", schedule);
                return GetById(schedule.ScheduleId);
            }
        }

        public bool Delete(int scheduleId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Schedule WHERE ScheduleId = @ScheduleId", new { ScheduleId = scheduleId });
                return affectedRows > 0;
            }
        }
    }
}
