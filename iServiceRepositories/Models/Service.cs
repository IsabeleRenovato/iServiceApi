namespace iServiceRepositories.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        public int EstablishmentProfileID { get; set; }
        public int ServiceCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public byte[] ServiceImage { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Excluded { get; set; }

        // Relações
        public virtual EstablishmentProfile EstablishmentProfile { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
    }

}
