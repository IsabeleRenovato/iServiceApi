namespace iServiceRepositories.Repositories.Models.Request
{
    public class SpecialDayModel
    {
        public int EstablishmentProfileID { get; set; }
        public DateTime Date { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public string? BreakStart { get; set; }
        public string? BreakEnd { get; set; }
    }
}
