using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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

        public List<AppointmentStatus> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<AppointmentStatus>("SELECT AppointmentStatusID, Name, CreationDate, LastUpdateDate FROM AppointmentStatus").AsList();
            }
        }

        public AppointmentStatus GetById(int appointmentStatusId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<AppointmentStatus>("SELECT AppointmentStatusID, Name, CreationDate, LastUpdateDate FROM AppointmentStatus WHERE AppointmentStatusID = @AppointmentStatusID", new { AppointmentStatusID = appointmentStatusId });
            }
        }

        public AppointmentStatus Insert(AppointmentStatusModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO AppointmentStatus (Name) VALUES (@Name); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public AppointmentStatus Update(AppointmentStatus appointmentStatus)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE AppointmentStatus SET Name = @Name, LastUpdateDate = NOW() WHERE AppointmentStatusID = @AppointmentStatusID", appointmentStatus);
                return GetById(appointmentStatus.AppointmentStatusId);
            }
        }

        public bool Delete(int appointmentStatusId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM AppointmentStatus WHERE AppointmentStatusID = @AppointmentStatusID", new { AppointmentStatusID = appointmentStatusId });
                return affectedRows > 0;
            }
        }
    }
}
