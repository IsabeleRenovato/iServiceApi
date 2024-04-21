using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
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
                return connection.Query<Appointment>("SELECT AppointmentId, ServiceId, ClientProfileId, EstablishmentProfileId, AppointmentStatusId, Start, End, CreationDate, LastUpdateDate FROM Appointment").AsList();
            }
        }

        public Appointment GetById(int appointmentId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Appointment>("SELECT AppointmentId, ServiceId, ClientProfileId, EstablishmentProfileId, AppointmentStatusId, Start, End, CreationDate, LastUpdateDate FROM Appointment WHERE AppointmentId = @AppointmentId", new { AppointmentId = appointmentId });
            }
        }

        public List<Appointment> GetByEstablishmentAndDate(int establishmentProfileId, DateTime start)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Appointment>(
                    "SELECT AppointmentId, ServiceId, EstablishmentProfileId, ClientProfileId, Start, End, CreationDate, LastUpdateDate " +
                    "FROM Appointment WHERE EstablishmentProfileId = @EstablishmentProfileId AND CAST(Start AS DATE) = CAST(@Start AS DATE)",
                    new { EstablishmentProfileId = establishmentProfileId, Start = start }).AsList();
            }
        }

        public Appointment Insert(AppointmentModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Appointment (ServiceId, ClientProfileId, EstablishmentProfileId, AppointmentStatusId, Start, End) VALUES (@ServiceId, @ClientProfileId, @EstablishmentProfileId, @AppointmentStatusId, @Start, @End); SELECT LAST_INSERT_Id();", model);
                return GetById(id);
            }
        }

        public Appointment Update(Appointment appointment)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Appointment SET ServiceId = @ServiceId, ClientProfileId = @ClientProfileId, EstablishmentProfileId = @EstablishmentProfileId, AppointmentStatusId = @AppointmentStatusId, Start = @Start, End = @End, LastUpdateDate = NOW() WHERE AppointmentId = @AppointmentId", appointment);
                return GetById(appointment.AppointmentId);
            }
        }

        public bool Delete(int appointmentId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Appointment WHERE AppointmentId = @AppointmentId", new { AppointmentId = appointmentId });
                return affectedRows > 0;
            }
        }
    }
}
