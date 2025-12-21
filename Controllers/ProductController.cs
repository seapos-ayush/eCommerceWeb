using eCommerceWeb.Enums;
using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;
using eCommerceWeb.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceWeb.Controllers
{
    [Route("home/product")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("details/{category}/{id}")]
        public async Task<IActionResult> ProductDetails(string category, int id)
        {
            var allProduct = await _productService.GetAllProductList();
            if (allProduct == null)
            {
                return NotFound();
            }

            var product = allProduct.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            //Getting ProductList By StatusId and CategoryId To Show Related New Products 
            var user = ViewBag.userDetailsSession as LoginResponse;

            //if (user == null)
            //    return RedirectToAction("Login", "Account");

            var userId = user?.Id ?? 1;
            var isVendor = user?.UserType == "Vendor";

            if (user?.UserType == "") // user?.UserType == "" means guest user or customer
            {
                userId = 1;
            }

            int newProductStatusId = (int)DbStatic.NewProducts;

            if (isVendor)
            {
                newProductStatusId = (int)VendorProductStatus.statusId2;
            }

            var req = new GetProductListByStatusIdAndCatgId_Request
            {
                StatusId = newProductStatusId,
                CategoryId = category == "men" ? 1 : 2,
                UserId = userId
            };
            var result = await _productService.GetProductListByStatusIdAndCatgId(req);
            ViewBag.productListByStatusIdAndCatgIdData = result.Where(p => p.Id != id).ToList();

            return View(product);
        }

        [HttpGet("collection/{category}/{categoryId}")]
        public async Task<IActionResult> Collection(string category, int categoryId)
        {
            ViewBag.Category = category;   
            ViewBag.CategoryId = categoryId;

            var user = ViewBag.userDetailsSession as LoginResponse;

            //if (user == null)
            //    return RedirectToAction("Login", "Account");

            var userId = user?.Id ?? 1;
            var isVendor = user?.UserType == "Vendor";
            if (user?.UserType == "") // user?.UserType == "" means guest user or customer
            {
                userId = 1;
            }

            int newProductStatusId = (int)DbStatic.NewProducts;


            //New Added
            var req1 = new GetAllByUserId
            {
                UserId = userId
            };
            var categoriesResponse = await _productService.GetCategoryAsync(req1);
            ViewBag.categories = categoriesResponse?.Data ?? Enumerable.Empty<CategoryItemsResponse>();

            //if (isVendor)
            //{
            //    newProductStatusId = (int)VendorProductStatus.statusId2;
            //}
            if (isVendor)
            {
                var productStatusRes = await _productService.GetProductStatusMappingUserWiseAsync(req1);
                var list = productStatusRes?.ToList() ?? new List<ProductStatusResponse>();
                ViewBag.ProductStatus = list;
                if (list.Count >= 2)
                {
                    newProductStatusId = (int)(list[2]?.Id ?? newProductStatusId);
                }
            }




            // Getting ProductList By CategoryId To Show Products in Collection Page
            var req = new GetProductListByCategoryId_Request
            {
                CategoryId = categoryId,
                UserId = userId
            };
            var ProductListByCategoryData = await _productService.GetProductListByCategoryId(req);
            ViewBag.ProductListByCategoryData = ProductListByCategoryData;

            // Getting ProductList By StatusId and CategoryId To Show Related New Products
            var req2 = new GetProductListByStatusIdAndCatgId_Request
            {
                StatusId = newProductStatusId,
                CategoryId = categoryId,
                UserId = userId
            };
            var result = await _productService.GetProductListByStatusIdAndCatgId(req2);
            ViewBag.productListByStatusIdAndCatgIdData = result;


            //Getting Unique Brands
            var uniqueBrands = new HashSet<string>();
            foreach (var product in ProductListByCategoryData)
            {
                if (product.Tags != null)
                {
                    foreach (var brand in product.Tags)
                    {
                        uniqueBrands.Add(brand);
                    }
                }
            }
            ViewBag.UniqueBrands = uniqueBrands;

            //Getting Unique Colors
            var uniqueColors = new HashSet<string>();
            foreach (var product in ProductListByCategoryData)
            {
                if (product.Colors != null)
                {
                    foreach (var color in product.Colors)
                    {
                        uniqueColors.Add(color);
                    }
                }
            }
            ViewBag.UniqueColors = uniqueColors;

            return View();

        }

        [HttpPost("productfilterbybrandandcolor")]
        public async Task<IActionResult> ProductFilterByBrandAndColor([FromBody] FilterRequest request)
        {
            var user = ViewBag.userDetailsSession as LoginResponse;
            //if (user == null)
            //    return RedirectToAction("Login", "Account");

            var userId = user?.Id ?? 1;

            if (user?.UserType == "") // user?.UserType == "" means guest user or customer
            {
                userId = 1;
            }

            var req = new GetProductListByCategoryId_Request
            {
                CategoryId = request.CategoryId,
                UserId = userId
            };

            var allProducts = await _productService.GetProductListByCategoryId(req);
            if (allProducts == null)
            {
                return NotFound();
            }

            //var filteredProducts = allProducts.Where(p =>
            //    (request.Brands == null || request.Brands.Count == 0 || (p.Brands != null && p.Brands.Any(b => request.Brands.Contains(b)))) &&
            //    (string.IsNullOrEmpty(request.Color) || (p.Colors != null && p.Colors.Contains(request.Color))) &&
            //    p.Price >= request.MinPrice &&
            //    p.Price <= request.MaxPrice
            //).ToList();


            var filteredProducts = allProducts.Where(p =>
            {
                // Check brand match
                bool brandMatch = true;
                if(request.Brands != null && request.Brands.Count > 0 && request.Brands.Any())
                {
                    brandMatch = p.Tags != null && p.Tags.Any(b => request.Brands.Contains(b));
                }

                // Check color match
                bool colorMatch = true;
                if(!string.IsNullOrEmpty(request.Color))
                {
                    colorMatch = p.Colors != null && p.Colors.Contains(request.Color);
                }

                // Check price range match
                bool priceMatch = p.SalePrice >= request.MinPrice && p.Price <= request.MaxPrice;

                return brandMatch && colorMatch && priceMatch;
            }).ToList();

            return PartialView("_FetchProducts", filteredProducts);
        }
    }
}
