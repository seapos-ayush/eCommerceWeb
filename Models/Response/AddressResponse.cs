using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.Models.Response
{
    public class AddressResponse
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public int IsDefault { get; set; }
    }
}
