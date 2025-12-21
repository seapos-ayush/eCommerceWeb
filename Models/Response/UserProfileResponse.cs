namespace eCommerceWeb.Models.Response
{
    public class UserProfileResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public DateTime Dob { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }

    }
}
