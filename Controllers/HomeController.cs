using eCommerceWeb.Enums;
using eCommerceWeb.Models;
using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;
using eCommerceWeb.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;

namespace eCommerceWeb.Controllers
{
    //[Route("home")]
    public class HomeController : BaseController
    {

        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        //[HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var homeBanners = new[]
            {
                    new {
                        Title = "Men's Summer Collection",
                        Subtitle = "welcome to fashion",
                        ImagePath = "assets/images/home-banner/1.jpg",
                        TargetUrl = Url.Action("Collection", "Product", new { category = "men", categoryId = 1 }),
                        CtaText = "shop now"
                    },
                    new {
                        Title = "Women's New Arrivals",
                        Subtitle = "welcome to fashion",
                        ImagePath = "assets/images/home-banner/2.jpg",
                        TargetUrl = Url.Action("Collection", "Product", new { category = "women", categoryId = 2 }),
                        CtaText = "shop now"
                    },
                      new {
                        Title = "Women's New Arrivals",
                        Subtitle = "welcome to fashion",
                        ImagePath = "assets/images/home-banner/3.jpg",
                        TargetUrl = Url.Action("Collection", "Product", new { category = "women", categoryId = 2 }),
                        CtaText = "shop now"
                    },
                        new {
                        Title = "Women's New Arrivals",
                        Subtitle = "welcome to fashion",
                        ImagePath = "assets/images/home-banner/4.jpg",
                        TargetUrl = Url.Action("Collection", "Product", new { category = "women", categoryId = 2 }),
                        CtaText = "shop now"
                    }
            };
            ViewBag.homeBanners = homeBanners;


            var user = ViewBag.userDetailsSession as LoginResponse;

            var userId = user?.Id ?? 1;
            var isVendor = user?.UserType == "Vendor";

            if(user?.UserType == "") // user?.UserType == "" means guest user or customer
            {
                userId = 1;
            }

            
            var req1 = new GetAllByUserId
            {
                UserId = userId
            };

            // Fetch Category List
            var categoriesResponse = await _productService.GetCategoryAsync(req1);
            ViewBag.categories = categoriesResponse?.Data ?? Enumerable.Empty<CategoryItemsResponse>();

            // Fetch Brand List
            var brandList = await _productService.GetAllBrandLogo(req1);
            if(brandList?.Data != null)
            {
                //ViewBag.BrandList = brandList.Data;
                var brandLogoJson = brandList.Data.GetRawText();
                ViewBag.BrandList = JsonConvert.DeserializeObject<dynamic>(brandLogoJson);
            } else
            {
                ViewBag.BrandList = null;
            }

            //Fetch Best Sellers and New Products
            int topCollectionStatusId = (int)DbStatic.TopCollection;
            int bestSellerStatusId = (int)DbStatic.BestSellers;
            int newProductsStatusId = (int)DbStatic.NewProducts;

            if (isVendor)
            {
                //bestSellerStatusId = (int)VendorProductStatus.statusId4;
                //newProductsStatusId = (int)VendorProductStatus.statusId5;

                var productStatusRes = await _productService.GetProductStatusMappingUserWiseAsync(req1);
                var list = productStatusRes?.ToList() ?? new List<ProductStatusResponse>();
                ViewBag.ProductStatus = list;
                if (list.Count >= 2)
                {
                    topCollectionStatusId = (int)(list[0]?.Id ?? topCollectionStatusId);
                    bestSellerStatusId = (int)(list[1]?.Id ?? bestSellerStatusId);
                    newProductsStatusId = (int)(list[2]?.Id ?? newProductsStatusId);

                }
            }
            //Fetch  Top Collection Products or Any Vendor Specific
            var req2 = new ProductRequestModel
            {
                UserId = userId,
                StatusId = topCollectionStatusId
            };
            var topCollection = await _productService.GetAllProductsAsync(req2);
            ViewData["TopCollection"] = topCollection;

            //Fetch Best Seller Products or Any Vendor Specific
            var req3 = new ProductRequestModel
            {
                UserId = userId,
                StatusId = bestSellerStatusId
            };
            var bestSellerRes = await _productService.GetAllProductsAsync(req3);
            ViewBag.bestSellerData = bestSellerRes;

            //Fetch New Products or Any Vendor Specific
            var req4 = new ProductRequestModel
            {
                UserId = userId,
                StatusId = newProductsStatusId
            };
            var newProductsRes = await _productService.GetAllProductsAsync(req4);
            ViewBag.newProductsData = newProductsRes;

            return View(topCollection);



            ////Fetch  Top Collection
            //var req2 = new ProductRequestModel
            //{
            //    UserId = userId,
            //    StatusId = (int)DbStatic.TopCollection
            //};

            //var topCollection = await _productService.GetAllProductsAsync(req2);
            //ViewData["TopCollection"] = topCollection;

            ////Fetch Best Sellers and New Products
            //int bestSellerStatusId = (int)DbStatic.BestSellers;
            //int newProductsStatusId = (int)DbStatic.NewProducts;

            //if(isVendor)
            //{
            //    bestSellerStatusId = (int)VendorProductStatus.statusId4;
            //    newProductsStatusId = (int)VendorProductStatus.statusId5;
            //}
            //var req3 = new ProductRequestModel
            //{
            //    UserId = userId,
            //    StatusId = bestSellerStatusId
            //};

            //var bestSellerRes = await _productService.GetAllProductsAsync(req3);
            //ViewBag.bestSellerData = bestSellerRes;


            //var req4 = new ProductRequestModel
            //{
            //    UserId = userId,
            //    StatusId = newProductsStatusId
            //};
            //var newProductsRes = await _productService.GetAllProductsAsync(req4);
            //ViewBag.newProductsData = newProductsRes;

            //return View(topCollection);
        }

        [HttpGet("about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

      
    }
}
