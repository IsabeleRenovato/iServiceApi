namespace iServiceRepositories.Repositories.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        public int EstablishmentProfileID { get; set; }
        public int ServiceCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDuration { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
