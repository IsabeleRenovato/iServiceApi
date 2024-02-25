namespace iServiceRepositories.Models
{
    public class UserInfo
    {
        public User User { get; set; }
        public UserRole UserRole { get; set; }
        public EstablishmentProfile? EstablishmentProfile { get; set; }
        public ClientProfile? ClientProfile { get; set; }
        public Address? Address { get; set; }
        public string? Token { get; set; }
    }
}
