using Dapper;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using Microsoft.Extensions.Configuration;

namespace iServiceRepositories.Repositories
{
    public class FeedbackRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly MySqlConnectionSingleton _connectionSingleton;

        public FeedbackRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionSingleton = new MySqlConnectionSingleton(_connectionString);
        }

        public List<Feedback> Get()
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Feedback>("SELECT FeedbackId, AppointmentId, Description, Rating, CreationDate, LastUpdateDate FROM Feedback").AsList();
            }
        }

        public Feedback GetById(int feedbackId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Feedback>("SELECT FeedbackId, AppointmentId, Description, Rating, CreationDate, LastUpdateDate FROM Feedback WHERE FeedbackId = @FeedbackId", new { FeedbackId = feedbackId });
            }
        }

        public List<Feedback> GetByEstablishmentProfileId(int establishmentProfileId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.Query<Feedback>("SELECT F.FeedbackId, F.AppointmentId, F.Description, F.Rating, F.CreationDate, F.LastUpdateDate  FROM Feedback F INNER JOIN Appointment A ON F.AppointmentId = A.AppointmentId WHERE A.EstablishmentProfileId = @EstablishmentProfileId", new { EstablishmentProfileId = establishmentProfileId }).AsList();
            }
        }

        public Feedback Insert(FeedbackModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Feedback (AppointmentId, Description, Rating) VALUES (@AppointmentId, @Description, @Rating); SELECT LAST_INSERT_Id();", model);
                return GetById(id);
            }
        }

        public Feedback Update(Feedback feedback)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Feedback SET AppointmentId = @AppointmentId, Description = @Description, Rating = @Rating, LastUpdateDate = NOW() WHERE FeedbackId = @FeedbackId", feedback);
                return GetById(feedback.FeedbackId);
            }
        }

        public bool Delete(int feedbackId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Feedback WHERE FeedbackId = @FeedbackId", new { FeedbackId = feedbackId });
                return affectedRows > 0;
            }
        }
    }

}
