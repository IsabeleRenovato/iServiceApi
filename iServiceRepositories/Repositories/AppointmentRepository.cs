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
                return connection.Query<Appointment>("SELECT AppointmentID, ServiceID, ClientProfileID, EstablishmentProfileID, AppointmentStatusID, Start, End, CreationDate, LastUpdateDate FROM Appointment").AsList();
            }
        }

        public Appointment GetById(int appointmentID)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Appointment>("SELECT AppointmentID, ServiceID, ClientProfileID, EstablishmentProfileID, AppointmentStatusID, Start, End, CreationDate, LastUpdateDate FROM Appointment WHERE AppointmentID = @AppointmentID", new { AppointmentID = appointmentID });
            }
        }

        public List<Appointment> GetByEstablishmentAndDate(int establishmentProfileId, DateTime start)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Appointment>(
                    "SELECT AppointmentID, ServiceId, EstablishmentProfileID, ClientProfileID, Start, End, CreationDate, LastUpdateDate " +
                    "FROM Appointment WHERE EstablishmentProfileID = @EstablishmentProfileID AND CAST(Start AS DATE) = CAST(@Start AS DATE)",
                    new { EstablishmentProfileID = establishmentProfileId, Start = start }).AsList();
            }
        }

        public Appointment Insert(AppointmentModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Appointment (ServiceID, ClientProfileID, EstablishmentProfileID, AppointmentStatusID, Start, End) VALUES (@ServiceID, @ClientProfileID, @EstablishmentProfileID, @AppointmentStatusID, @Start, @End); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public Appointment Update(Appointment appointment)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Appointment SET ServiceID = @ServiceID, ClientProfileID = @ClientProfileID, EstablishmentProfileID = @EstablishmentProfileID, AppointmentStatusID = @AppointmentStatusID, Start = @Start, End = @End, LastUpdateDate = NOW() WHERE AppointmentID = @AppointmentID", appointment);
                return GetById(appointment.AppointmentID);
            }
        }

        public bool Delete(int appointmentID)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Appointment WHERE AppointmentID = @AppointmentID", new { AppointmentID = appointmentID });
                return affectedRows > 0;
            }
        }
    }
}
