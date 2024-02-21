namespace iServiceRepositories.Models
{
    public class UserRole
    {
        public int? UserRoleID { get; set; }
        public string? Role { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool? Excluded { get; set; }
    }
}
