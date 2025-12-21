namespace eCommerceWeb.Models.Response
{
    public class ProductStatusResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string CreatedOn { get; set; } = string.Empty;
        public string? ModifiedOn { get; set; }
    }
}
