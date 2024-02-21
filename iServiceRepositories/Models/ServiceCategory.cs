namespace iServiceRepositories.Models
{
    public class ServiceCategory
    {
        public int ServiceCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Excluded { get; set; }
    }

}
