namespace iServiceRepositories.Repositories.Models
{
    public class ServiceCategory
    {
        public int ServiceCategoryId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
