namespace iServiceRepositories.Repositories.Models
{
    public class EstablishmentProfile
    {
        public int EstablishmentProfileId { get; set; }
        public int UserId { get; set; }
        public string CNPJ { get; set; }
        public string CommercialName { get; set; }
        public int EstablishmentCategoryId { get; set; }
        public int AddressId { get; set; }
        public string Description { get; set; }
        public string CommercialPhone { get; set; }
        public string CommercialEmail { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
