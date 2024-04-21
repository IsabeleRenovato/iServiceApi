namespace iServiceRepositories.Repositories.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int EstablishmentProfileId { get; set; }
        public string Days { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string? BreakStart { get; set; }
        public string? BreakEnd { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }

}
