using iServiceRepositories.Repositories.Models;
using Newtonsoft.Json;

namespace iServiceServices.Services.Models
{
    public class UserInfo
    {
        public User? User { get; set; }
        public UserRole? UserRole { get; set; }
        [JsonProperty("establishmentProfile", NullValueHandling = NullValueHandling.Ignore)]
        public EstablishmentProfile? EstablishmentProfile { get; set; }
        [JsonProperty("clientProfile", NullValueHandling = NullValueHandling.Ignore)]
        public ClientProfile? ClientProfile { get; set; }
        public Address? Address { get; set; }
        public string? Token { get; set; }
    }
}
