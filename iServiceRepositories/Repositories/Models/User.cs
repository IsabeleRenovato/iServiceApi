namespace iServiceRepositories.Repositories.Models
{
    public class User
    {
        public int UserID { get; set; }
        public int UserRoleID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
