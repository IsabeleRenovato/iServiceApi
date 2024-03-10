using iServiceRepositories.Repositories.Models.Request;

namespace iServiceServices.Services.Models.Auth
{
    public class Register
    {
        public int UserId { get; set; }
        public EstablishmentProfileModel? EstablishmentProfile { get; set; }
        public ClientProfileModel? ClientProfile { get; set; }
        public AddressModel Address { get; set; }
    }
}
