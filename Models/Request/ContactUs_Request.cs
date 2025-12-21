using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.Models.Request
{
    public class ContactUs_Request
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile Number must be exactly 10 digits")]
        public string? MobileNo { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? EmailId { get; set; }


        [Required(ErrorMessage = "Message is required")]
        public string? Message { get; set; }


    }
}
