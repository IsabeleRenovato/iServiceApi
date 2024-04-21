using Newtonsoft.Json;

namespace iServiceRepositories.Repositories.Models.Request
{
    public class ClientProfileModel
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public string CPF { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public byte[]? Photo { get; set; }
    }
}
