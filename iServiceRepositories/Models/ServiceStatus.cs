namespace iServiceRepositories.Models
{
    public class ServiceStatus
    {
        public int ServiceStatusID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Excluded { get; set; }
    }

}
