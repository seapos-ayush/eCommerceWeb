namespace eCommerceWeb.Models.Request
{
    public class GetProductListByStatusIdAndCatgId_Request
    {
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
    }
}
