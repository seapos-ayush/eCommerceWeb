using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;
using eCommerceWeb.Services.Interface;

namespace eCommerceWeb.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public UserService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config["ApiBaseUrls:ProductApi"]);
        }

        public async Task<ApiResponse> AddCustomerAddress(AddressRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerAddress/Save", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to save customer address." } };
            }
        }

        public async Task<ApiResponse<List<AddressResponse>>> GetCustomerAddress(GetAllByUserId model)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<AddressResponse>>>($"CustomerAddress/GetAllByCustomerId/{model.UserId}");
            if (response != null)
            {
                return response;
            }
            else
            {
                return new ApiResponse<List<AddressResponse>> { IsSuccess = false, Errors = new List<string> { "Failed to retrieve customer addresses." } };
            }
        }

        public async Task<ApiResponse<UserProfileResponse>> GetuserDeatils(GetAllByUserId model)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerMaster/GetByUserId", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserProfileResponse>>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse<UserProfileResponse> { IsSuccess = false, Errors = new List<string> { "Failed to retrieve user details." } };
            }
        }

        public async Task<LoginResponse?> Login(LoginRequest req)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerMaster/Login", req);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                if (loginResponse != null && loginResponse.IsSuccess)
                {
                    return loginResponse.Data;
                }
            }
            return null;
        }

        public async Task<ApiResponse> RemoveAddress(RemoveAddress_Request model)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerAddress/Delete", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to remove address." } };
            }

        }

        public async Task<ApiResponse> SaveContactUs(ContactUs_Request model)
        {
            var response = await _httpClient.PostAsJsonAsync("ContactUs/Save", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to save contact us message." } };
            }
        }

        public async Task<ApiResponse> SetDefaultAddress(SetDefaultAddressRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerAddress/UpdateDefaultAddress", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to set default address." } };
            }
        }

        public async Task<ApiResponse> UpdateCustomerAddress(AddressRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerAddress/Update", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to update customer address." } };
            }
        }

        public async Task<ApiResponse> UserRegistration(UserRegistration_Request model)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerMaster/Save", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to register user." } };
            }
        }
        public async Task<LoginResponse?> VendorLogin(LoginRequest req)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerMaster/VendorLogin", req);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                if (loginResponse != null && loginResponse.IsSuccess)
                {
                    return loginResponse.Data;
                }
            }
            return null;
        }
        public async Task<ApiResponse> UpdateProfile(UserProfileRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("CustomerMaster/UpdateProfile", model);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return apiResponse;
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Errors = new List<string> { "Failed to update profile." } };
            }
        }
    }
}
