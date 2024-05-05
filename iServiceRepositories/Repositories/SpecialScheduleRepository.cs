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

        public List<SpecialSchedule> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<SpecialSchedule>("SELECT SpecialScheduleId, UserProfileId, Date, Start, End, BreakStart, BreakEnd, Active, Deleted, CreationDate, LastUpdateDate FROM SpecialSchedule").AsList();
            }
        }

        public SpecialSchedule GetById(int specialScheduleId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<SpecialSchedule>("SELECT SpecialScheduleId, UserProfileId, Date, Start, End, BreakStart, BreakEnd, Active, Deleted, CreationDate, LastUpdateDate FROM SpecialSchedule WHERE SpecialScheduleId = @SpecialScheduleId", new { SpecialScheduleId = specialScheduleId });
            }
        }

        public SpecialSchedule Insert(SpecialScheduleInsert specialScheduleModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO SpecialSchedule (UserProfileId, Date, Start, End, BreakStart, BreakEnd) VALUES (@UserProfileId, @Date, @Start, @End, @BreakStart, @BreakEnd); SELECT LAST_INSERT_Id();", specialScheduleModel);
                return GetById(id);
            }
        }

        public SpecialSchedule Update(SpecialScheduleUpdate specialScheduleUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE SpecialSchedule SET Date = @Date, Start = @Start, End = @End, BreakStart = @BreakStart, BreakEnd = @BreakEnd, LastUpdateDate = NOW() WHERE SpecialScheduleId = @SpecialScheduleId", specialScheduleUpdateModel);
                return GetById(specialScheduleUpdateModel.SpecialScheduleId);
            }
        }

        public void SetActiveStatus(int specialScheduleId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE SpecialSchedule SET Active = @IsActive WHERE SpecialScheduleId = @SpecialScheduleId", new { IsActive = isActive, SpecialScheduleId = specialScheduleId });
            }
        }

        public void SetDeletedStatus(int specialScheduleId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE SpecialSchedule SET Deleted = @IsDeleted WHERE SpecialScheduleId = @SpecialScheduleId", new { IsDeleted = isDeleted, SpecialScheduleId = specialScheduleId });
            }
        }
    }
}
