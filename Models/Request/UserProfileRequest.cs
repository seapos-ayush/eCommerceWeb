using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.Models.Request
{
    public class UserProfileRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }  // shown as disabled

        [Required(ErrorMessage = "Mobile is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Mobile { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        public bool GenderId { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
    }
}
