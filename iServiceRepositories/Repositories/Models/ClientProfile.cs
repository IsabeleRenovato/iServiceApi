namespace iServiceRepositories.Repositories.Models
{
    public class ClientProfile
    {
        public int ClientProfileId { get; set; }
        public int UserId { get; set; }
        public string CPF { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public int AddressId { get; set; }
        public string? Photo { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
