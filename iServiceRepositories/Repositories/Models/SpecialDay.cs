namespace iServiceRepositories.Repositories.Models
{
    public class SpecialDay
    {
        public int SpecialDayId { get; set; }
        public int EstablishmentProfileId { get; set; }
        public DateTime Date { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public string? BreakStart { get; set; }
        public string? BreakEnd { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
