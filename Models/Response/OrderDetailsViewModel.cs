using eCommerceWeb.Models.Request;

namespace eCommerceWeb.Models.Response
{
    public class OrderDetailsViewModel
    {
        public List<ProductSessionModel> Product { get; set; }
        public PaymentDataRequest ShippingDetails { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
