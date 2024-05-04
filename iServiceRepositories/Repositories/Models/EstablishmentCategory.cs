namespace iServiceRepositories.Repositories.Models
{
    public class EstablishmentCategory
    {
        public int EstablishmentCategoryId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
    public class EstablishmentCategoryInsert
    {
        public string Name { get; set; }
    }
    public class EstablishmentCategoryUpdate
    {
        public int EstablishmentCategoryId { get; set; }
        public string Name { get; set; }
    }
}
