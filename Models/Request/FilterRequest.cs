namespace eCommerceWeb.Models.Request
{
    public class FilterRequest
    {
        public List<string> Brands { get; set; } = new List<string>();
        public int CategoryId { get; set; }
        public string? Color { get; set; } 
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}
