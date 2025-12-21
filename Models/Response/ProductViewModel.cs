namespace eCommerceWeb.Models.Response
{
    public class ProductResponse
    {
        public List<ProductViewModel> Products { get; set; } = new();
    }
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Discount { get; set; }
        public string ShortDetails { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int IsNew { get; set; }
        public int IsSale { get; set; }
        public int SizeId { get; set; }
        public string Category { get; set; }

        public List<string> Pictures { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Size { get; set; }
        public List<string> Tags { get; set; }
        //public List<string> Brands { get; set; }

        public List<ProductVariant> Variants { get; set; }
    }

    public class ProductVariant
    {
        public string Color { get; set; }
        public string Images { get; set; }
    }
}
