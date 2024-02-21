namespace iServiceRepositories.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int AppointmentID { get; set; }
        public DateTime FeedbackDateTime { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Excluded { get; set; }

        // Relações
        public virtual Appointment Appointment { get; set; }
    }

}
