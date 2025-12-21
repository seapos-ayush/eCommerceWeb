using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;
using eCommerceWeb.Services.Interface;

namespace eCommerceWeb.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ProductService(HttpClient httpClient,IConfiguration config) {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config["ApiBaseUrls:ProductApi"]);
        }
        public async Task<ApiResponse> CancelOrders(CancelOrderRequest model)
        {
            var response  = await _httpClient.PostAsJsonAsync("OrderMaster/CancelOrderByCustomer", model);
            if(response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to cancel order." } };
            }
        }

        public async Task<ApiResponse> GetAllBrandLogo(GetAllByUserId model)
        {
            var response = await _httpClient.PostAsJsonAsync("Brandlogo/GetAll", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to retrieve brand logos." } };
            }

        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductList()
        {
            var response = await _httpClient.GetAsync("ProductMaster/GetProductList");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
                return products ?? Enumerable.Empty<ProductViewModel>();
            }
            else
            {
                return Enumerable.Empty<ProductViewModel>();
            }
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync(ProductRequestModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("ProductMaster/GetProductListByStatusId", model);
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
                return products ?? Enumerable.Empty<ProductViewModel>();
            }
            else
            {
                return Enumerable.Empty<ProductViewModel>();
            }
        }

        public async Task<ApiResponse<List<CustomerOrderDetails_Response>>> GetAll_CustomerOrderDetails_ByUserId(CustomerOrderDetails_Request model)
        {
           var response = await _httpClient.PostAsJsonAsync("OrderMaster/GetAll_CustomerOrderDetails_ByUserId", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<CustomerOrderDetails_Response>>>();
                return apiResponse ?? new ApiResponse<List<CustomerOrderDetails_Response>> { IsSuccess = false, Errors = new List<string> { "No data found." } };
            }
            else
            {
                return new ApiResponse<List<CustomerOrderDetails_Response>> { IsSuccess = false, Errors = new List<string> { "Failed to retrieve order details." } };
            }
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductListByCategoryId(GetProductListByCategoryId_Request model)
        {
           var response = await _httpClient.PostAsJsonAsync("ProductMaster/GetProductListByCategoryId", model);
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
                return products ?? Enumerable.Empty<ProductViewModel>();
            }
            else
            {
                return Enumerable.Empty<ProductViewModel>();
            }
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductListByStatusIdAndCatgId(GetProductListByStatusIdAndCatgId_Request model)
        {
            var response = await _httpClient.PostAsJsonAsync("ProductMaster/GetProductListByStatusIdAndCatgId", model);
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
                return products ?? Enumerable.Empty<ProductViewModel>();
            }
            else
            {
                return Enumerable.Empty<ProductViewModel>();
            }
        }

        public async Task<ApiResponse> GetProductStatus(GetAllByUserId model)
        {
           var response = await _httpClient.PostAsJsonAsync("ProductMasterStatus/GetAll", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to retrieve product statuses." } };
            }
        }

        public async Task<ApiResponse<List<GetAll_CustomerOrderDetails_Response>>> GetPurchaseOrderDetails(GetAllByUserId model)
        {
            var response = await _httpClient.PostAsJsonAsync("OrderMaster/GetAll_CustomerOrderDetails", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<GetAll_CustomerOrderDetails_Response>>>();
                return apiResponse ?? new ApiResponse<List<GetAll_CustomerOrderDetails_Response>> { IsSuccess = false, Errors = new List<string> { "No data found." } };
            }
            else
            {
                return new ApiResponse<List<GetAll_CustomerOrderDetails_Response>> { IsSuccess = false, Errors = new List<string> { "Failed to retrieve purchase order details." } };
            }
        }

        public async Task<ApiResponse> SavePaymentMaster(PaymentDataRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("PaymentMaster/Save", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to save payment data." } };
            }
        }


        //Added New
        public async Task<ApiResponse<IEnumerable<CategoryItemsResponse>>> GetCategoryAsync(GetAllByUserId model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Category/GetAll", model);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<CategoryItemsResponse>>>();
                    return apiResponse ?? new ApiResponse<IEnumerable<CategoryItemsResponse>>
                    {
                        Data = Enumerable.Empty<CategoryItemsResponse>(),
                        IsSuccess = false,
                        Errors = ["Empty response from server."]
                    };
                }
                else
                {
                    // Optionally log response.StatusCode / reason
                    return new ApiResponse<IEnumerable<CategoryItemsResponse>>
                    {
                        Data = Enumerable.Empty<CategoryItemsResponse>(),
                        IsSuccess = false,
                        Errors = [$"Request failed with status code: {response.StatusCode}"]
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CategoryItemsResponse>>
                {
                    Data = Enumerable.Empty<CategoryItemsResponse>(),
                    IsSuccess = false,
                    Errors = [$"An error occurred: {ex.Message}"]
                };
            }

        }
        public async Task<IEnumerable<ProductStatusResponse>> GetProductStatusMappingUserWiseAsync(GetAllByUserId model)
        {
            var response = await _httpClient.PostAsJsonAsync("ProductMasterStatus/GetProductStatusMappingUserWise", model);

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<ProductStatusResponse>();

            var apiResponse = await response.Content
                                            .ReadFromJsonAsync<ApiResponse<IEnumerable<ProductStatusResponse>>>();

            return apiResponse?.Data ?? Enumerable.Empty<ProductStatusResponse>();
        }
    }
}
