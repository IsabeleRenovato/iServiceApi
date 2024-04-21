namespace iServiceRepositories.Repositories.Models.Request
{
    public class ScheduleModel
    {
        public int EstablishmentProfileId { get; set; }
        public string Days { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string? BreakStart { get; set; }
        public string? BreakEnd { get; set; }
    }
}
