namespace iServiceRepositories.Repositories.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int EstablishmentProfileID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; } // Em minutos
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
