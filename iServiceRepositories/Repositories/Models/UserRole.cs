namespace iServiceRepositories.Repositories.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public string Role { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }

}
