using eCommerceWeb.Helpers;
using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;
using eCommerceWeb.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWeb.Controllers
{
    [Route("home/account")]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View(new LoginRequest());
        }

        [HttpPost("submit-login")]
        public async Task<IActionResult> SubmitLogin(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return View("Login", model);

            var user = await _userService.Login(model);
            if (user != null)
            {
                //HttpContext.Session.SetString("userDetailsSession", JsonConvert.SerializeObject(user));
                HttpContext.Session.SetObject("userDetailsSession", user);

                return RedirectToAction("Index", "Home");
            }
            TempData["ToastError"] = "Invalid username or password.";
            return View("Login", model);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userDetailsSession");
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("vendor-login")]
        public IActionResult VendorLogin()
        {
            return View(new LoginRequest());
        }

        [HttpPost("submit-vendor-login")]
        public async Task<IActionResult> SubmitVendorLogin(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return View("VendorLogin", model);

            var user = await _userService.VendorLogin(model);
            if (user != null)
            {
                HttpContext.Session.SetObject("userDetailsSession", user);
                return RedirectToAction("Index", "Home");
            }
            TempData["ToastError"] = "Invalid username or password.";
            return View("VendorLogin", model);
        }


        [HttpGet("registration")]
        public IActionResult Registration()
        {
            return View(new UserRegistration_Request());
        }

        [HttpPost("submit-registration")]
        public async Task<IActionResult> SubmitRegistration(UserRegistration_Request model)
        {
            if (!ModelState.IsValid)
                return View("Registration", model);

            var result = await _userService.UserRegistration(model);
            if (result != null && result.IsSuccess)
            {
                TempData["ToastSuccess"] = "Registration successful. Please login to continue.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["ToastError"] = string.Join(", ", result?.Errors ?? new List<string>());
                return View("Registration", model);
            }
        }


        [HttpGet("myaccount")]
        public IActionResult MyAccount()
        {
            return View();
        }

        [HttpGet("myaddress")]
        public async Task<IActionResult> MyAddress()
        {
            if (ViewBag.userDetailsSession == null)
                return RedirectToAction("Login", "Account");

            //var loginResponse = ((LoginResponse)ViewBag.userDetailsSession);
            var loginResponse = ViewBag.userDetailsSession as LoginResponse;

            if (loginResponse == null)
                return RedirectToAction("Login", "Account");

            var model = new GetAllByUserId
            {
                UserId = loginResponse.Id
            };
            var addresses = await _userService.GetCustomerAddress(model);
            if (!addresses.IsSuccess)
            {
                TempData["ToastError"] = string.Join(", ", addresses.Errors ?? new List<string>());
                return View(new List<AddressResponse>());
            }
            return View(addresses.Data);
        }
        [HttpPost("remove-address")]
        public async Task<IActionResult> RemoveAddress(RemoveAddress_Request model)
        {
            if (ViewBag.userDetailsSession == null)
                return RedirectToAction("Login", "Account");

            var result = await _userService.RemoveAddress(model);
            if (result != null && result.IsSuccess)
            {
                TempData["ToastSuccess"] = "Address removed successfully.";
            }
            else
            {
                TempData["ToastError"] = string.Join(", ", result?.Errors ?? new List<string>());
            }
            return RedirectToAction("MyAddress", "Account");
        }
        [HttpGet("myprofile")]
        public async Task<IActionResult> MyProfile()
        {
            if (ViewBag.userDetailsSession == null)
                return RedirectToAction("Login", "Account");

            var userDetails = ViewBag.userDetailsSession as LoginResponse;
            var req = new GetAllByUserId
            {
                UserId = userDetails.Id
            };

            var result = await _userService.GetuserDeatils(req);
            if (result != null && result.IsSuccess)
            {
                var res = new UserProfileRequest
                {
                    Id = result.Data.Id,
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    Email = result.Data.EmailId,
                    Dob = result.Data.Dob,
                    Gender = result.Data.Gender,
                    GenderId = result.Data.GenderId == 1 ? true : false
                };
                return View(res);
            }
            else
            {
                TempData["ToastError"] = string.Join(", ", result?.Errors ?? new List<string>());
                return View();
            }
        }
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile(UserProfileRequest model)
        {
            if (ViewBag.userDetailsSession == null)
                return RedirectToAction("Login", "Account");
            if (!ModelState.IsValid)
                return View("MyProfile", model);
            var result = await _userService.UpdateProfile(model);
            if (result != null && result.IsSuccess)
            {
                TempData["ToastSuccess"] = "Profile updated successfully.";
                return RedirectToAction("MyProfile", "Account");
            }
            else
            {
                TempData["ToastError"] = string.Join(", ", result?.Errors ?? new List<string>());
                return View("MyProfile", model);
            }
        }
    }
}
