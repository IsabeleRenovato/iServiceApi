namespace iServiceRepositories.Repositories.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public int EstablishmentProfileID { get; set; }
        public int ClientProfileID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
