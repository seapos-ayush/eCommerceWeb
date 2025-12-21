namespace eCommerceWeb.Models.Response
{
    public class GetAll_CustomerOrderDetails_Response
    {
        public int BillId { get; set; }
        public string ProductName { get; set; }
        public string ShortDetails { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; }
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
        public string PaymentType { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentStatus { get; set; }
        public int PaymentStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string CancelReason { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime ShippedOn { get; set; }
        public DateTime DeliveredOn { get; set; }
    }
}
