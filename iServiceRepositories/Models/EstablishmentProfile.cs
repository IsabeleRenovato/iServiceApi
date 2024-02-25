namespace iServiceRepositories.Models
{
    public class EstablishmentProfile
    {
        public int? EstablishmentProfileID { get; set; }
        public int? UserID { get; set; }
        public string CNPJ { get; set; }
        public string CommercialName { get; set; }
        public int? AddressID { get; set; }
        public string Description { get; set; }
        public string CommercialPhone { get; set; }
        public string CommercialEmail { get; set; }
        public byte[]? Logo { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool? Excluded { get; set; }
    }
}
