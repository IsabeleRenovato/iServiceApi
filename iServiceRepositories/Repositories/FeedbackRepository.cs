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

        public async Task<List<Feedback>> GetAsync()
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<Feedback>("SELECT * FROM Feedback");
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Feedback> GetByIdAsync(int feedbackId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                return await connection.QueryFirstOrDefaultAsync<Feedback>(
                    "SELECT * FROM Feedback WHERE FeedbackId = @FeedbackId", new { FeedbackId = feedbackId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<Feedback>> GetFeedbackByUserProfileIdAsync(int userProfileId)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var queryResult = await connection.QueryAsync<Feedback>(
                    "SELECT F.FeedbackId, F.AppointmentId, F.Description, F.Rating, F.Active, F.Deleted, F.CreationDate, F.LastUpdateDate FROM Feedback F INNER JOIN Appointment A ON A.AppointmentId = F.AppointmentId WHERE A.EstablishmentUserProfileId = @EstablishmentUserProfileId",
                    new { EstablishmentUserProfileId = userProfileId });
                return queryResult.AsList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Feedback> InsertAsync(Feedback feedbackModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>(
                    "INSERT INTO Feedback (AppointmentId, Description, Rating) VALUES (@AppointmentId, @Description, @Rating); SELECT LAST_INSERT_Id();", feedbackModel);
                return await GetByIdAsync(id);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Feedback> UpdateAsync(Feedback feedbackUpdateModel)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Feedback SET Description = @Description, Rating = @Rating, LastUpdateDate = NOW() WHERE FeedbackId = @FeedbackId", feedbackUpdateModel);
                return await GetByIdAsync(feedbackUpdateModel.FeedbackId);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetActiveStatusAsync(int feedbackId, bool isActive)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Feedback SET Active = @IsActive WHERE FeedbackId = @FeedbackId", new { IsActive = isActive, FeedbackId = feedbackId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task SetDeletedStatusAsync(int feedbackId, bool isDeleted)
        {
            var connection = await _connectionSingleton.GetConnectionAsync();
            try
            {
                await connection.ExecuteAsync(
                    "UPDATE Feedback SET Deleted = @IsDeleted WHERE FeedbackId = @FeedbackId", new { IsDeleted = isDeleted, FeedbackId = feedbackId });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
