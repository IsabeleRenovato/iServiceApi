namespace iServiceRepositories.Repositories.Models.Request
{
    public class FeedbackModel
    {
        public int AppointmentId { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
    }
}
