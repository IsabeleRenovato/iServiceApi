namespace iServiceRepositories.Models
{
    public class ServiceAvailability
    {
        public int? ServiceAvailabilityID { get; set; }
        public int ServiceID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan AvailableHours { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool? Excluded { get; set; }
    }
}
