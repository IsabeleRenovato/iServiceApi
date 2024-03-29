namespace iServiceRepositories.Repositories.Models.Request
{
    public class AppointmentModel
    {
        public int ServiceId { get; set; }
        public int EstablishmentProfileID { get; set; }
        public int ClientProfileID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
