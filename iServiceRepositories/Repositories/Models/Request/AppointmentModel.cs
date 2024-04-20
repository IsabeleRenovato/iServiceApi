namespace iServiceRepositories.Repositories.Models.Request
{
    public class AppointmentModel
    {
        public int ServiceID { get; set; }
        public int ClientProfileID { get; set; }
        public int EstablishmentProfileID { get; set; }
        public int AppointmentStatusID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
