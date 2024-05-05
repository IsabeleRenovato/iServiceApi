using iServiceRepositories.Repositories.Models;

namespace iServiceServices.Services.Models.Auth
{
    public class Register
    {
        public UserProfileInsert UserProfile { get; set; }
        public AddressInsert? Address { get; set; }
    }
}
