using eCommerceWeb.Helpers;
using eCommerceWeb.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWeb.Controllers
{
    [Route("home/wishlist")]
    public class WishlistController : BaseController
    {
        public IActionResult Index()
        {
            //var list = HttpContext.Request.GetObject<List<ProductSessionModel>>("WishList");
            var list = Request.GetObject<List<ProductSessionModel>>("WishList");
            return View(list ?? new List<ProductSessionModel>());
        }
        [HttpPost("add")]
        public IActionResult AddToWishlist(ProductSessionModel model)
        {
            var list = Request.GetObject<List<ProductSessionModel>>("WishList") ?? new List<ProductSessionModel>();
            if (list.Any(p => p.Id == model.Id))
            {
               TempData["ToastError"] = "Product is already in wishlist.";
            } else             {
                list.Add(model);
                Response.SetObject("WishList", list);
                TempData["ToastSuccess"] = "Product added to wishlist.";
            }
            ViewBag.wishlist = list;
           return Redirect(Request.Headers["Referer"].ToString());
           //return RedirectToAction("Index");
        }
        [HttpPost("remove")]
        public IActionResult RemoveFromWishlist(int productId)
        {
            var list = Request.GetObject<List<ProductSessionModel>>("WishList") ?? new List<ProductSessionModel>();
            var itemToRemove = list.FirstOrDefault(p => p.Id == productId);
            if (itemToRemove != null)
            {
                list.Remove(itemToRemove);
                Response.SetObject("WishList", list);
                TempData["ToastSuccess"] = "Product removed from wishlist.";
            }
            else
            {
                TempData["ToastError"] = "Product not found in wishlist.";
            }
            ViewBag.wishlist = list;
            return Redirect(Request.Headers["Referer"].ToString());
            //return RedirectToAction("Index");
        }
    }
}
