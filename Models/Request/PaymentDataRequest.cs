namespace eCommerceWeb.Models.Request
{
    public class PaymentDataRequest
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string Postalcode { get; set; }
        public string Amount { get; set; }
        public string ShippingAmount { get; set; }
        public int PaymentTypeId { get; set; }
        public int PaymentStatusId { get; set; }
        public Purchaseitems[]? Items { get; set; }
        public StripePaymentRequest? Payment { get; set; }
        public int CreatedBy { get; set; }
    }

    public class Purchaseitems
    {
        public int ProductId { get; set; }
        public string Quantity { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public string Discount { get; set; }
    }
    public class StripePaymentRequest
    {
        public int Amount { get; set; }
        public string Description { get; set; }
        public string TokenId { get; set; }
    }
}
