namespace iServiceRepositories.Repositories.Models
{
    public class SpecialSchedule
    {
        public int SpecialScheduleId { get; set; }
        public int EstablishmentUserId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
    public class SpecialScheduleInsert
    {
        public int EstablishmentUserId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
    }
    public class SpecialScheduleUpdate
    {
        public int SpecialScheduleId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
    }
}
