using eCommerceWeb.Models.Request;
using eCommerceWeb.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWeb.Controllers
{
    [Route("home/contactus")]
    public class ContactUsController : BaseController
    {
        private readonly IUserService _userService;
        public ContactUsController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(new ContactUs_Request());
        }
        [HttpPost("submit")]
        public async Task<IActionResult> Submit(ContactUs_Request model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var result = await _userService.SaveContactUs(model);
            if (result != null && result.IsSuccess)
            {
                TempData["ToastSuccess"] = "Your message has been sent successfully.";
                return RedirectToAction("Index", new ContactUs_Request());
            }
            else
            {
                TempData["ToastError"] = string.Join(", ", result?.Errors ?? new List<string>());
                return View("Index", model);
            }
        }
    }
}
