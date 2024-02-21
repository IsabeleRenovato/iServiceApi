namespace iServiceRepositories.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string AdditionalInfo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Excluded { get; set; }
    }

}
