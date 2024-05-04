using Dapper;
using iServiceRepositories.Repositories.Models;
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
                return connection.Query<Feedback>("SELECT FeedbackId, AppointmentId, Description, Rating, Active, Deleted, CreationDate, LastUpdateDate FROM Feedback").AsList();
            }
        }

        public Feedback GetById(int feedbackId)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                return connection.QueryFirstOrDefault<Feedback>("SELECT FeedbackId, AppointmentId, Description, Rating, Active, Deleted, CreationDate, LastUpdateDate FROM Feedback WHERE FeedbackId = @FeedbackId", new { FeedbackId = feedbackId });
            }
        }

        public Feedback Insert(FeedbackInsert feedbackModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                var id = connection.QuerySingle<int>("INSERT INTO Feedback (AppointmentId, Description, Rating) VALUES (@AppointmentId, @Description, @Rating); SELECT LAST_INSERT_Id();", feedbackModel);
                return GetById(id);
            }
        }

        public Feedback Update(FeedbackUpdate feedbackUpdateModel)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Feedback SET AppointmentId = @AppointmentId, Description = @Description, Rating = @Rating, LastUpdateDate = NOW() WHERE FeedbackId = @FeedbackId", feedbackUpdateModel);
                return GetById(feedbackUpdateModel.FeedbackId);
            }
        }

        public void SetActiveStatus(int feedbackId, bool isActive)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Feedback SET Active = @IsActive WHERE FeedbackId = @FeedbackId", new { IsActive = isActive, FeedbackId = feedbackId });
            }
        }

        public void SetDeletedStatus(int feedbackId, bool isDeleted)
        {
            using (var connection = _connectionSingleton.GetConnection())
            {
                connection.Execute("UPDATE Feedback SET Deleted = @IsDeleted WHERE FeedbackId = @FeedbackId", new { IsDeleted = isDeleted, FeedbackId = feedbackId });
            }
        }
    }
}
