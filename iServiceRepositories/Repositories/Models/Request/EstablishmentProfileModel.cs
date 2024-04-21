using Newtonsoft.Json;

namespace iServiceRepositories.Repositories.Models.Request
{
    public class EstablishmentProfileModel
    {
        public int UserId { get; set; }
        public string CNPJ { get; set; }
        public string CommercialName { get; set; }
        public int EstablishmentCategoryId { get; set; }
        public int? AddressId { get; set; }
        public string Description { get; set; }
        public string CommercialPhone { get; set; }
        public string CommercialEmail { get; set; }
        public byte[]? Photo { get; set; }
    }
}
