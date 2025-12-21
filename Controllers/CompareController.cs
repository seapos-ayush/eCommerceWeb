using eCommerceWeb.Helpers;
using eCommerceWeb.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWeb.Controllers
{
    [Route("home/compare")]
    public class CompareController : BaseController
    {
        public IActionResult Index()
        {
            var list = Request.GetObject<List<ProductSessionModel>>("CompareList");
            return View(list ?? new List<ProductSessionModel>());
        }
        [HttpPost("add")]
        public IActionResult AddToCompare(ProductSessionModel model)
        {
            var list = Request.GetObject<List<ProductSessionModel>>("CompareList") ?? new List<ProductSessionModel>();
            if (list.Any(p => p.Id == model.Id))
            {
                TempData["ToastError"] = "Product is already in compare list.";
            }
            else
            {
                list.Add(model);
                Response.SetObject("CompareList", list);
                TempData["ToastSuccess"] = "Product added to compare list.";
            }
            ViewBag.compareList = list;
            //return RedirectToAction("Index");
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpPost("remove")]
        public IActionResult RemoveFromCompare(int productId)
        {
            var list = Request.GetObject<List<ProductSessionModel>>("CompareList") ?? new List<ProductSessionModel>();
            var itemToRemove = list.FirstOrDefault(p => p.Id == productId);
            if (itemToRemove != null)
            {
                list.Remove(itemToRemove);
                Response.SetObject("CompareList", list);
                TempData["ToastSuccess"] = "Product removed from compare list.";
            }
            else
            {
                TempData["ToastError"] = "Product not found in compare list.";
            }
            ViewBag.compareList = list;
            return RedirectToAction("Index");
        }
    }
}
