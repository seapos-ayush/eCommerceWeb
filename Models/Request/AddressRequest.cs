using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.Models.Request
{
    public class AddressRequest
    {
        public int Id { get; set; } = 0;
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name must contain letters only")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last name must contain letters only")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please select a country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [RegularExpression(@"^\d{1,6}$", ErrorMessage = "Postal code must be up to 6 digits")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        public int IsDefault { get; set; }
        public bool SetIsDefault { get; set; }
    }
}
