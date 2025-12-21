namespace eCommerceWeb.Models.Request
{
    public class ProductSessionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Price { get; set; }
        public string Pictures { get; set; }
        public string ShortDetails { get; set; }
        public int Stock { get; set; }
        public int? Quantity { get; set; } = 1;
        public string? Colors { get; set; }
        public string? Size { get; set; }
        public string? Category { get; set; }
    }
}
