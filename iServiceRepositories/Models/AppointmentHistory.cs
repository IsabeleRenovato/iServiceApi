namespace iServiceRepositories.Models
{
    public class AppointmentHistory
    {
        public int AppointmentHistoryID { get; set; }
        public int AppointmentID { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public int PreviousServiceStatusID { get; set; }
        public int NewServiceStatusID { get; set; }
        public string Remarks { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Excluded { get; set; }

        // Relações
        public virtual Appointment Appointment { get; set; }
        public virtual ServiceStatus PreviousServiceStatus { get; set; }
        public virtual ServiceStatus NewServiceStatus { get; set; }
    }

}
