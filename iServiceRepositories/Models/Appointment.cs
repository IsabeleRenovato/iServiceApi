namespace iServiceRepositories.Models
{
    public class Appointment
    {
        public int? AppointmentID { get; set; }
        public int ClientProfileID { get; set; }
        public int ServiceID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ServiceStatusID { get; set; }
        public string ClientComments { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool? Excluded { get; set; }
    }
}
