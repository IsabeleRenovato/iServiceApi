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
                return connection.Query<Appointment>("SELECT AppointmentId, ServiceId, EstablishmentProfileID, ClientProfileID, StartDateTime, EndDateTime, CreationDate, LastUpdateDate FROM Appointment").AsList();
            }
        }

        public Appointment GetById(int appointmentId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Appointment>("SELECT AppointmentId, ServiceId, EstablishmentProfileID, ClientProfileID, StartDateTime, EndDateTime, CreationDate, LastUpdateDate FROM Appointment WHERE AppointmentId = @AppointmentId", new { AppointmentId = appointmentId });
            }
        }

        public List<Appointment> GetByEstablishmentAndDate(int establishmentProfileId, DateTime startDateTime)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Appointment>(
                    "SELECT AppointmentId, ServiceId, EstablishmentProfileID, ClientProfileID, StartDateTime, EndDateTime, CreationDate, LastUpdateDate " +
                    "FROM Appointment WHERE EstablishmentProfileID = @EstablishmentProfileID AND CAST(StartDateTime AS DATE) = CAST(@StartDateTime AS DATE)",
                    new { EstablishmentProfileID = establishmentProfileId, StartDateTime = startDateTime }).AsList();
            }
        }

        public Appointment Insert(AppointmentModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Appointment (ServiceId, EstablishmentProfileID, ClientProfileID, StartDateTime, EndDateTime) VALUES (@ServiceId, @EstablishmentProfileID, @ClientProfileID, @StartDateTime, @EndDateTime); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public Appointment Update(Appointment appointment)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Appointment SET ServiceId = @ServiceId, EstablishmentProfileID = @EstablishmentProfileID, ClientProfileID = @ClientProfileID, StartDateTime = @StartDateTime, EndDateTime = @EndDateTime, LastUpdateDate = NOW() WHERE AppointmentId = @AppointmentId", appointment);
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
