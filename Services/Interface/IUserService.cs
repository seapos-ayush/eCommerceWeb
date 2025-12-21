using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;

namespace eCommerceWeb.Services.Interface
{
    public interface IUserService
    {
        Task<LoginResponse?> Login(LoginRequest req);
        Task<LoginResponse?> VendorLogin(LoginRequest req);

        Task<ApiResponse> UserRegistration(UserRegistration_Request model);
        Task<ApiResponse> SaveContactUs(ContactUs_Request model);
        Task<ApiResponse> AddCustomerAddress(AddressRequest model);
        Task<ApiResponse> UpdateCustomerAddress(AddressRequest model);
        Task<ApiResponse> SetDefaultAddress(SetDefaultAddressRequest model);
        Task<ApiResponse> RemoveAddress(RemoveAddress_Request model);
        Task<ApiResponse<UserProfileResponse>> GetuserDeatils(GetAllByUserId model);
        Task<ApiResponse<List<AddressResponse>>> GetCustomerAddress(GetAllByUserId model);

        Task<ApiResponse> UpdateProfile(UserProfileRequest model);

    }
}
