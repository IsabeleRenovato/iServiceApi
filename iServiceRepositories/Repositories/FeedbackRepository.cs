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
                return connection.Query<Feedback>("SELECT FeedbackID, AppointmentID, Description, Rating, CreationDate, LastUpdateDate FROM Feedback").AsList();
            }
        }

        public Feedback GetById(int feedbackId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Feedback>("SELECT FeedbackID, AppointmentID, Description, Rating, CreationDate, LastUpdateDate FROM Feedback WHERE FeedbackID = @FeedbackID", new { FeedbackID = feedbackId });
            }
        }

        public Feedback Insert(FeedbackModel model)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Feedback (AppointmentID, Description, Rating) VALUES (@AppointmentID, @Description, @Rating); SELECT LAST_INSERT_ID();", model);
                return GetById(id);
            }
        }

        public Feedback Update(Feedback feedback)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Feedback SET AppointmentID = @AppointmentID, Description = @Description, Rating = @Rating, LastUpdateDate = NOW() WHERE FeedbackID = @FeedbackID", feedback);
                return GetById(feedback.FeedbackID);
            }
        }

        public bool Delete(int feedbackId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                int affectedRows = connection.Execute("DELETE FROM Feedback WHERE FeedbackID = @FeedbackID", new { FeedbackID = feedbackId });
                return affectedRows > 0;
            }
        }
    }

}
