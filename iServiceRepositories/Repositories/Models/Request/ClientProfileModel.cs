using Newtonsoft.Json;

namespace iServiceRepositories.Repositories.Models.Request
{
    public class ClientProfileModel
    {
        public int UserID { get; set; }
        public string CPF { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public int? AddressID { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }

}
