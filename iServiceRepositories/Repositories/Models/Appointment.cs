namespace iServiceRepositories.Repositories.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int ServiceID { get; set; }
        public int ClientProfileID { get; set; }
        public int EstablishmentProfileID { get; set; }
        public int AppointmentStatusID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
