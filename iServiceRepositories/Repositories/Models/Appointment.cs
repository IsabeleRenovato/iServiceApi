namespace iServiceRepositories.Repositories.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public int ClientProfileId { get; set; }
        public int EstablishmentProfileId { get; set; }
        public int AppointmentStatusId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
