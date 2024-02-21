namespace iServiceRepositories.Models
{
    public class BusinessHours
    {
        public int BusinessHoursID { get; set; }
        public int EstablishmentProfileID { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public TimeSpan BreakIntervals { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Excluded { get; set; }

        // Relações
        public virtual EstablishmentProfile EstablishmentProfile { get; set; }
    }

}
