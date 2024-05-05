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

        public List<Appointment> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Appointment>("SELECT AppointmentId, ServiceId, ClientUserProfileId, EstablishmentUserProfileId, AppointmentStatusId, Start, End, Active, Deleted, CreationDate, LastUpdateDate FROM Appointment").AsList();
            }
        }

        public Appointment GetById(int appointmentId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Appointment>("SELECT AppointmentId, ServiceId, ClientUserProfileId, EstablishmentUserProfileId, AppointmentStatusId, Start, End, Active, Deleted, CreationDate, LastUpdateDate FROM Appointment WHERE AppointmentId = @AppointmentId", new { AppointmentId = appointmentId });
            }
        }

        public Appointment Insert(AppointmentInsert appointmentModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Appointment (ServiceId, ClientUserProfileId, EstablishmentUserProfileId, AppointmentStatusId, Start, End) VALUES (@ServiceId, @ClientUserProfileId, @EstablishmentUserProfileId, @AppointmentStatusId, @Start, @End); SELECT LAST_INSERT_Id();", appointmentModel);
                return GetById(id);
            }
        }

        public Appointment Update(AppointmentUpdate appointmentUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Appointment SET AppointmentStatusId = @AppointmentStatusId, Start = @Start, End = @End, LastUpdateDate = NOW() WHERE AppointmentId = @AppointmentId", appointmentUpdateModel);
                return GetById(appointmentUpdateModel.AppointmentId);
            }
        }

        public void SetActiveStatus(int appointmentId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Appointment SET Active = @IsActive WHERE AppointmentId = @AppointmentId", new { IsActive = isActive, AppointmentId = appointmentId });
            }
        }

        public void SetDeletedStatus(int appointmentId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Appointment SET Deleted = @IsDeleted WHERE AppointmentId = @AppointmentId", new { IsDeleted = isDeleted, AppointmentId = appointmentId });
            }
        }
    }
}
