namespace iServiceRepositories.Repositories.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int AppointmentID { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
