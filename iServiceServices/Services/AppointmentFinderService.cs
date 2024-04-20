using iServiceRepositories.Repositories.Models;

namespace iServiceServices.Services
{
    public class AppointmentFinderService
    {
        public List<TimeSpan> FindAvailableSlots(Schedule schedule, List<SpecialDay> specialDays, Service service, DateTime date, List<Appointment> existingAppointments)
        {
            List<TimeSpan> availableSlots = new List<TimeSpan>();
            // Lógica adaptada para usar strings convertidas para TimeSpan
            var dayOfWeek = (int)date.DayOfWeek;
            var specialDay = specialDays.FirstOrDefault(sd => sd.Date.Date == date.Date);

            // Verifica se o dia é um dia de funcionamento normal
            if (!schedule.Days.Contains(dayOfWeek.ToString())) return new List<TimeSpan>();

            // Converte horários de string para TimeSpan
            var start = ParseTime(specialDay != null ? specialDay.Start : schedule.Start);
            var end = ParseTime(specialDay != null ? specialDay.End : schedule.End);
            var breakStart = string.IsNullOrEmpty(specialDay?.BreakStart) ? (TimeSpan?)null : ParseTime(specialDay.BreakStart);
            var breakEnd = string.IsNullOrEmpty(specialDay?.BreakEnd) ? (TimeSpan?)null : ParseTime(specialDay.BreakEnd);

            TimeSpan currentTime = start;
            while (currentTime.Add(TimeSpan.FromMinutes(service.EstimatedDuration)) <= end)
            {
                var potentialEndTime = currentTime.Add(TimeSpan.FromMinutes(service.EstimatedDuration));

                if (breakStart.HasValue && breakEnd.HasValue && currentTime < breakEnd && potentialEndTime > breakStart)
                {
                    currentTime = breakEnd.Value > currentTime ? breakEnd.Value : currentTime.Add(TimeSpan.FromMinutes(30));
                    continue;
                }

                var isTimeSlotAvailable = !existingAppointments.Any(app =>
                    app.Start.Date == date.Date &&
                    (currentTime < app.End.TimeOfDay && potentialEndTime > app.Start.TimeOfDay));

                if (isTimeSlotAvailable)
                {
                    availableSlots.Add(currentTime);
                }

                currentTime = currentTime.Add(TimeSpan.FromMinutes(30));
            }

            return availableSlots;
        }
        private TimeSpan ParseTime(string time)
        {
            return TimeSpan.Parse(time);
        }
    }
}
