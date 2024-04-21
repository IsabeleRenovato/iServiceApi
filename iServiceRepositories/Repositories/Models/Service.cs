namespace iServiceRepositories.Repositories.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int EstablishmentProfileId { get; set; }
        public int ServiceCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDuration { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
