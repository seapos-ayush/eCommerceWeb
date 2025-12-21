using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;

namespace eCommerceWeb.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync(ProductRequestModel model);
        Task<IEnumerable<ProductViewModel>> GetAllProductList();
        Task<IEnumerable<ProductViewModel>> GetProductListByCategoryId(GetProductListByCategoryId_Request model);
        Task<IEnumerable<ProductViewModel>> GetProductListByStatusIdAndCatgId(GetProductListByStatusIdAndCatgId_Request model);

        Task<ApiResponse> GetAllBrandLogo(GetAllByUserId model);
        Task<ApiResponse> GetProductStatus(GetAllByUserId model);
        Task<ApiResponse> SavePaymentMaster(PaymentDataRequest model);
        Task<ApiResponse> CancelOrders(CancelOrderRequest model);
        Task<ApiResponse<List<CustomerOrderDetails_Response>>> GetAll_CustomerOrderDetails_ByUserId(CustomerOrderDetails_Request model);
        Task<ApiResponse<List<GetAll_CustomerOrderDetails_Response>>> GetPurchaseOrderDetails(GetAllByUserId model);


        //Added New
        Task<ApiResponse<IEnumerable<CategoryItemsResponse>>> GetCategoryAsync(GetAllByUserId model);
        Task<IEnumerable<ProductStatusResponse>> GetProductStatusMappingUserWiseAsync(GetAllByUserId model);
    }
}
