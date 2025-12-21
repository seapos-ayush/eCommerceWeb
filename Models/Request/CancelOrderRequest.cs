namespace eCommerceWeb.Models.Request
{
    public class CancelOrderRequest
    {
        public int BillId { get; set; }
        public string CancelReason { get; set; }
    }
}
