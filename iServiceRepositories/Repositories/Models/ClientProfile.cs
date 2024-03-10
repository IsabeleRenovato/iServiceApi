namespace iServiceRepositories.Repositories.Models
{
    public class ClientProfile
    {
        public int ClientProfileID { get; set; }
        public int UserID { get; set; }
        public string CPF { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public int AddressID { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }

}
