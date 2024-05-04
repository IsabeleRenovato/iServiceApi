﻿namespace iServiceRepositories.Repositories.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int EstablishmentUserId { get; set; }
        public string Days { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
    public class ScheduleInsert
    {
        public int EstablishmentUserId { get; set; }
        public string Days { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
    }
    public class ScheduleUpdate
    {
        public int ScheduleId { get; set; }
        public string Days { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
    }
}
