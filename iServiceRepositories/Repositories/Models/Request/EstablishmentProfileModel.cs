using Newtonsoft.Json;

namespace iServiceRepositories.Repositories.Models.Request
{
    public class EstablishmentProfileModel
    {
        public int UserID { get; set; }
        public string CNPJ { get; set; }
        public string CommercialName { get; set; }
        public int EstablishmentCategoryID { get; set; }
        public int? AddressID { get; set; }
        public string Description { get; set; }
        public string CommercialPhone { get; set; }
        public string CommercialEmail { get; set; }
        public byte[]? Photo { get; set; }
    }
}
