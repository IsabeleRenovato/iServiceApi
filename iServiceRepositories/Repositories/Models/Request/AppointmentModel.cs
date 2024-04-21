namespace iServiceRepositories.Repositories.Models.Request
{
    public class AppointmentModel
    {
        public int ServiceId { get; set; }
        public int ClientProfileId { get; set; }
        public int EstablishmentProfileId { get; set; }
        public int AppointmentStatusId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
