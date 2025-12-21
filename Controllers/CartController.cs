using eCommerceWeb.Helpers;
using eCommerceWeb.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace eCommerceWeb.Controllers
{
    [Route("home/cart")]
    public class CartController : BaseController
    {
        public IActionResult Index()
        {
            var list = Request.GetObject<List<ProductSessionModel>>("CartList");
            return View(list ?? new List<ProductSessionModel>());
        }
        [HttpPost("add")]
        public IActionResult AddToCart(ProductSessionModel model)
        {
            var list = Request.GetObject<List<ProductSessionModel>>("CartList") ?? new List<ProductSessionModel>();
            if (list.Any(p => p.Id == model.Id))
            {
                TempData["ToastError"] = "Product is already in cart.";
            }
            else
            {
                list.Add(model);
                Response.SetObject("CartList", list);
                TempData["ToastSuccess"] = "Product added to cart.";


                // Remove from wishlist if exists start
                var wishlist = Request.GetObject<List<ProductSessionModel>>("WishList") ?? new List<ProductSessionModel>();
                var itemToRemove = wishlist.FirstOrDefault(p => p.Id == model.Id);
                if (itemToRemove != null)
                {
                    wishlist.Remove(itemToRemove);
                    Response.SetObject("WishList", wishlist);
                    TempData["ToastSuccess"] = "Product added to cart and removed from wishlist.";
                }
                // Remove from wishlist if exists end

            }
            ViewBag.cartList = list;
            //return RedirectToAction("Index");
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet("removecart/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            var list = Request.GetObject<List<ProductSessionModel>>("CartList") ?? new List<ProductSessionModel>();
            var itemToRemove = list.FirstOrDefault(p => p.Id == productId);
            if (itemToRemove != null)
            {
                list.Remove(itemToRemove);
                Response.SetObject("CartList", list);
                TempData["ToastSuccess"] = "Product removed from cart.";
            }
            else
            {
                TempData["ToastError"] = "Product not found in cart.";
            }
            ViewBag.cartList = list;
            //return RedirectToAction("Index");
            return Redirect(Request.Headers["Referer"].ToString());
        }


        [HttpPost]
        [Route("removecartitems")]
        public JsonResult RemoveCartItem([FromBody] ProductSessionModel request)
        {
            var cart = Request.GetObject<List<ProductSessionModel>>("CartList") ?? new List<ProductSessionModel>();
            var item = cart.FirstOrDefault(p => p.Id == request.Id);
            if (item != null)
            {
                cart.Remove(item);
                Response.SetObject("CartList", cart);
                ViewBag.cartList = cart;
                return Json(new { status = "success", count = cart.Count });
            }
            return Json(new { status = "error", message = "Not found", count = cart.Count });
        }

        [HttpPost]
        [Route("updatequantity")]
        public JsonResult UpdateQuantity([FromBody] ProductSessionModel product)
        {
            var cart = Request.GetObject<List<ProductSessionModel>>("CartList") ?? new List<ProductSessionModel>();
            var item = cart.FirstOrDefault(p => p.Id == product.Id);
            if (item != null && product.Quantity > 0)
            {
                item.Quantity = product.Quantity;
                Response.SetObject("CartList", cart);
                ViewBag.cartList = cart;
                return Json(new { status = "success" });
            }
            return Json(new { status = "error" });

        }
    }
}
