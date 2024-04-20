using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class SpecialDayRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public SpecialDayRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<SpecialDay> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<SpecialDay>("SELECT SpecialDayId, EstablishmentProfileID, Date, Start, End, BreakStart, BreakEnd, CreationDate, LastUpdateDate FROM SpecialDay").AsList();
            }
        }

        public SpecialDay GetById(int specialDayId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<SpecialDay>("SELECT SpecialDayId, EstablishmentProfileID, Date, Start, End, BreakStart, BreakEnd, CreationDate, LastUpdateDate FROM SpecialDay WHERE SpecialDayId = @SpecialDayId", new { SpecialDayId = specialDayId });
            }
        }

        public List<SpecialDay> GetByEstablishmentAndDate(int establishmentProfileId, DateTime date)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<SpecialDay>(
                    "SELECT SpecialDayId, EstablishmentProfileID, Date, Start, End, BreakStart, BreakEnd, CreationDate, LastUpdateDate " +
                    "FROM SpecialDay WHERE EstablishmentProfileID = @EstablishmentProfileID AND CAST(Date AS DATE) = CAST(@Date AS DATE)",
                    new { EstablishmentProfileID = establishmentProfileId, Date = date }).AsList();
            }
        }

        public SpecialDay Insert(SpecialDayModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO SpecialDay (EstablishmentProfileID, Date, Start, End, BreakStart, BreakEnd) VALUES (@EstablishmentProfileID, @Date, @Start, @End, @BreakStart, @BreakEnd); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public SpecialDay Update(SpecialDay specialDay)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE SpecialDay SET EstablishmentProfileID = @EstablishmentProfileID, Date = @Date, Start = @Start, End = @End, BreakStart = @BreakStart, BreakEnd = @BreakEnd, LastUpdateDate = NOW() WHERE SpecialDayId = @SpecialDayId", specialDay);
                return GetById(specialDay.SpecialDayId);
            }
        }

        public bool Delete(int specialDayId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM SpecialDay WHERE SpecialDayId = @SpecialDayId", new { SpecialDayId = specialDayId });
                return affectedRows > 0;
            }
        }
    }
}
