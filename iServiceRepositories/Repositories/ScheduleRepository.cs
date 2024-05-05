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

        public List<Schedule> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Schedule>("SELECT ScheduleId, UserProfileId, Days, Start, End, BreakStart, BreakEnd, Active, Deleted, CreationDate, LastUpdateDate FROM Schedule").AsList();
            }
        }

        public Schedule GetById(int scheduleId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Schedule>("SELECT ScheduleId, UserProfileId, Days, Start, End, BreakStart, BreakEnd, Active, Deleted, CreationDate, LastUpdateDate FROM Schedule WHERE ScheduleId = @ScheduleId", new { ScheduleId = scheduleId });
            }
        }

        public Schedule Insert(ScheduleInsert scheduleModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Schedule (UserProfileId, Days, Start, End, BreakStart, BreakEnd) VALUES (@UserProfileId, @Days, @Start, @End, @BreakStart, @BreakEnd); SELECT LAST_INSERT_Id();", scheduleModel);
                return GetById(id);
            }
        }

        public Schedule Update(ScheduleUpdate scheduleUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Schedule SET Days = @Days, Start = @Start, End = @End, BreakStart = @BreakStart, BreakEnd = @BreakEnd, LastUpdateDate = NOW() WHERE ScheduleId = @ScheduleId", scheduleUpdateModel);
                return GetById(scheduleUpdateModel.ScheduleId);
            }
        }

        public void SetActiveStatus(int scheduleId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Schedule SET Active = @IsActive WHERE ScheduleId = @ScheduleId", new { IsActive = isActive, ScheduleId = scheduleId });
            }
        }

        public void SetDeletedStatus(int scheduleId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Schedule SET Deleted = @IsDeleted WHERE ScheduleId = @ScheduleId", new { IsDeleted = isDeleted, ScheduleId = scheduleId });
            }
        }
    }
}
