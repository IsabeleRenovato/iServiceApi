namespace iServiceRepositories.Repositories.Models.Request
{
    public class ServiceModel
    {
        public int EstablishmentProfileID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; } // Em minutos
    }
}
