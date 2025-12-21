using eCommerceWeb.Helpers;
using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eCommerceWeb.Controllers
{
    public class BaseController : Controller
    {
       public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            ViewBag.cartList = httpContext.Request.GetObject<List<ProductSessionModel>>("cartList") ?? new List<ProductSessionModel>();
            ViewBag.wishList = httpContext.Request.GetObject<List<ProductSessionModel>>("wishList") ?? new List<ProductSessionModel>();
            ViewBag.compareList = httpContext.Request.GetObject<List<ProductSessionModel>>("compareList") ?? new List<ProductSessionModel>();


            var userSessionJson = httpContext.Session.GetString("userDetailsSession");
            if(!string.IsNullOrEmpty(userSessionJson))
            {
                var userDetails = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(userSessionJson);
                ViewBag.userDetailsSession = userDetails;
            } else
            {
                ViewBag.userDetailsSession = null;
            }

            // Common pre-action logic can be added here
            base.OnActionExecuting(context);
        }
    }
}
