namespace iServiceRepositories.Repositories.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public int ClientUserId { get; set; }
        public int EstablishmentUserId { get; set; }
        public int AppointmentStatusId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
    public class AppointmentInsert
    {
        public int ServiceId { get; set; }
        public int ClientUserId { get; set; }
        public int EstablishmentUserId { get; set; }
        public int AppointmentStatusId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
    public class AppointmentUpdate
    {
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public int ClientUserId { get; set; }
        public int EstablishmentUserId { get; set; }
        public int AppointmentStatusId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

}
