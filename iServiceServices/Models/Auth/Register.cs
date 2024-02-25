using iServiceRepositories.Models;

namespace iServiceServices.Models.Auth
{
    public class Register
    {
        public int UserId { get; set; }
        public EstablishmentProfile? Establishment { get; set; }
        public ClientProfile? Client { get; set; }
        public Address Address { get; set; }
    }
}
