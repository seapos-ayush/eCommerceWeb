using eCommerceWeb.Helpers;
using eCommerceWeb.Models.Request;
using eCommerceWeb.Models.Response;
using eCommerceWeb.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using System.Text.Json;

namespace eCommerceWeb.Controllers
{
    [Route("home/order")]
    public class OrderController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IConfiguration _config;//2025-12-12
        public OrderController(IUserService userService, IProductService productService, IConfiguration config)
        {
            _userService = userService;
            _productService = productService;
            _config = config;
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to proceed to checkout.";
                return RedirectToAction("Login", "Account");
            }

            var cart = Request.GetObject<List<ProductSessionModel>>("cartList") ?? new List<ProductSessionModel>();
            var userId = (ViewBag.userDetailsSession as LoginResponse).Id;
            var request = new GetAllByUserId { UserId = userId };
            var addressRes = await _userService.GetCustomerAddress(request);
            ViewBag.CustomerAddress = addressRes.IsSuccess ? addressRes.Data : new List<AddressResponse>();
            ViewBag.ApiBaseUrl = _config["ApiBaseUrls:ProductApi"];//2025-12-12
            return View(cart);
        }

        [HttpGet("add-address")]
        public IActionResult AddAddress(string returnUrl = null)
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to add an address.";
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(new AddressRequest());
        }
        [HttpPost("add-address")]
        public async Task<IActionResult> AddAddress(AddressRequest model, string returnUrl = null)
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to add an address.";
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            var user = ViewBag.userDetailsSession as LoginResponse;

            var req = new AddressRequest
            {
                Id = model.Id,
                CustomerId = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                CountryId = model.CountryId,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                IsDefault = model.SetIsDefault ? 1 : 0
            };

            ApiResponse? res = null;
            if (model.Id > 0)
            {
                res = await _userService.UpdateCustomerAddress(req);
            }
            else
            {
                res = await _userService.AddCustomerAddress(req);
            }

            if (res != null && res.IsSuccess)
            {
                TempData["ToastSuccess"] = model.Id > 0 ? "Address updated successfully." : "Address added successfully.";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Checkout", "Order");
            }
            else
            {
                TempData["ToastError"] = string.Join(", ", res?.Errors ?? new List<string>());
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
        }


        [HttpPost("set-default-address")]
        public async Task<IActionResult> SetDefaultAddress(int AddressId, int CustomerId, string ReturnUrl = null)
        {
            var request = new SetDefaultAddressRequest
            {
                AddressId = AddressId,
                CustomerId = CustomerId
            };
            var res = await _userService.SetDefaultAddress(request);
            if (res != null && res.IsSuccess)
            {
                TempData["ToastSuccess"] = "Default address updated successfully.";
            }
            else
            {
                TempData["ToastError"] = string.Join(", ", res.Errors);
            }
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return Redirect(ReturnUrl);

            return RedirectToAction("Checkout", "Order");
        }

        [HttpGet("edit-address")]
        public async Task<IActionResult> EditAddress(int Id, int UserId, string returnUrl = null)
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to edit an address.";
                return RedirectToAction("Login", "Account");
            }
            var request = new GetAllByUserId
            {
                UserId = UserId
            };
            var res = await _userService.GetCustomerAddress(request);

            var selectedAddress = res.Data.FirstOrDefault(a => a.AddressId == Id);
            if (selectedAddress == null)
            {
                TempData["ToastError"] = "Address not found.";
                return RedirectToAction("Checkout", "Order");
            }
            var model = new AddressRequest
            {
                Id = selectedAddress.AddressId,
                FirstName = selectedAddress.FirstName,
                LastName = selectedAddress.LastName,
                Phone = selectedAddress.Phone,
                CountryId = selectedAddress.CountryId,
                Address = selectedAddress.Address,
                City = selectedAddress.City,
                State = selectedAddress.State,
                PostalCode = selectedAddress.PostalCode,
                IsDefault = selectedAddress.IsDefault,
                SetIsDefault = selectedAddress.IsDefault == 1 ? true : false
            };
            ViewBag.ReturnUrl = returnUrl;
            return View("AddAddress", model);
        }

        [HttpGet("change-default-address")]
        public async Task<IActionResult> ChangeDefaultAddress(int Id, int UserId)
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to change default address.";
                return RedirectToAction("Login", "Account");
            }
            var req = new GetAllByUserId
            {
                UserId = UserId
            };
            var res = await _userService.GetCustomerAddress(req);
            if (res == null)
            {
                TempData["ToastError"] = "Unable to retrieve addresses.";
                return RedirectToAction("Checkout", "Order");
            }

            return View("AddAddress");

        }
        [HttpPost("cancel-order")]
        public async Task<IActionResult> CancelOrder([FromBody] CancelOrderRequest req)
        {
            var res = await _productService.CancelOrders(req);
            if (res != null && res.IsSuccess)
            {
                return Json(new { success = true, message = "Order cancelled successfully.", billId = req.BillId });
            }
            else
            {
                var errorMessage = res != null ? string.Join(", ", res.Errors) : "An error occurred while cancelling the order.";
                return Json(new { success = false, message = errorMessage });
            }
        }
        [HttpGet("my-order")]
        public async Task<IActionResult> MyOrders(int billId)
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to view your orders.";
                return RedirectToAction("Login", "Account");
            }
            var user = ViewBag.userDetailsSession as LoginResponse;
            if (user == null)
            {
                TempData["ToastError"] = "Please login to view your orders.";
                return RedirectToAction("Login", "Account");
            }

            var req = new GetAllByUserId
            {
                UserId = user.Id
            };
            var res = await _productService.GetPurchaseOrderDetails(req);
            if (res != null && res.IsSuccess)
            {
                var allOrders = res.Data as List<GetAll_CustomerOrderDetails_Response>;
                if (billId > 0)
                {
                    var productsInBill = allOrders.Where(o => o.BillId == billId).ToList();
                    if (productsInBill != null && productsInBill.Count == 0)
                    {
                        TempData["ToastError"] = "No products found for the specified order.";
                        return View(new List<GetAll_CustomerOrderDetails_Response>());
                    }
                    return View(productsInBill);
                }
            }
            else
            {
                TempData["ToastError"] = res != null ? string.Join(", ", res.Errors) : "An error occurred while retrieving your orders.";
                return View(new List<GetAll_CustomerOrderDetails_Response>());
            }

            return View(res.Data);
        }

        [HttpPost("details")]
        public async Task<IActionResult> OrderDetails(int billId)
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to view your orders.";
                return RedirectToAction("Login", "Account");
            }
            var user = ViewBag.userDetailsSession as LoginResponse;
            if (user == null)
            {
                TempData["ToastError"] = "Please login to view your orders.";
                return RedirectToAction("Login", "Account");
            }
            var req = new GetAllByUserId
            {
                UserId = user.Id
            };
            var res = await _productService.GetPurchaseOrderDetails(req);
            if (res != null && res.IsSuccess)
            {
                var allOrders = res.Data as List<GetAll_CustomerOrderDetails_Response>;
                var productsInBill = allOrders.Where(o => o.BillId == billId).ToList();
                if (productsInBill != null && productsInBill.Count == 0)
                {
                    TempData["ToastError"] = "No products found for the specified order.";
                    return View(new List<GetAll_CustomerOrderDetails_Response>());
                }
                return View(productsInBill);
            }
            else
            {
                TempData["ToastError"] = res != null ? string.Join(", ", res.Errors) : "An error occurred while retrieving your orders.";
                return View(new List<GetAll_CustomerOrderDetails_Response>());
            }
        }
        [HttpGet("payment-success")]
        public IActionResult PaymentSuccess()
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to proceed.";
                return RedirectToAction("Login", "Account");
            }
            if (TempData["OrderDetails"] is string json)
            {
                var model = JsonConvert.DeserializeObject<OrderDetailsViewModel>(json);
                return View(model);
            }

            return RedirectToAction("MyOrders");
        }
        [HttpGet("mypayments")]
        public async Task<IActionResult> MyPayments()
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to view your payments.";
                return RedirectToAction("Login", "Account");
            }
            var user = ViewBag.userDetailsSession as LoginResponse;
            if (user == null)
            {
                TempData["ToastError"] = "Please login to view your payments.";
                return RedirectToAction("Login", "Account");
            }
            var req = new GetAllByUserId
            {
                UserId = user.Id
            };
            var res = await _productService.GetPurchaseOrderDetails(req);
            if (res != null && res.IsSuccess)
            {
                return View(res.Data);
            }
            else
            {
                TempData["ToastError"] = res != null ? string.Join(", ", res.Errors) : "An error occurred while retrieving your payments.";
                return View(new List<GetAll_CustomerOrderDetails_Response>());
            }
        }
        [HttpGet("invoice")]
        public async Task<IActionResult> Invoice(int OrderId)
        {
            if (ViewBag.userDetailsSession == null)
            {
                TempData["ToastError"] = "Please login to view your payments.";
                return RedirectToAction("Login", "Account");
            }
            var user = ViewBag.userDetailsSession as LoginResponse;
            if (user == null)
            {
                TempData["ToastError"] = "Please login to view your payments.";
                return RedirectToAction("Login", "Account");
            }
            var req = new CustomerOrderDetails_Request
            {
                UserId = user.Id,
                OrderId = OrderId
            };
            var res = await _productService.GetAll_CustomerOrderDetails_ByUserId(req);
            if (res != null && res.IsSuccess)
            {
                return View(res.Data);
            }
            else
            {
                TempData["ToastError"] = res != null ? string.Join(", ", res.Errors) : "An error occurred while retrieving your invoice.";
                return View(new List<CustomerOrderDetails_Response>());
            }
        }

        //// Incomplete: SavePayment method
        //[HttpPost("save-payment")]
        //public async Task<IActionResult> SavePayment([FromBody] PaymentDataRequest data)
        //{
        //    if (data == null || string.IsNullOrEmpty(data.Payment?.TokenId))
        //    {
        //        return Json(new { success = false, message = "Invalid payment data." });
        //    }
        //    var res = await _productService.SavePaymentMaster(data);
        //    if (res != null && res.IsSuccess && res.Data is JsonElement element)
        //    {
        //        var model = new OrderDetailsViewModel
        //        {

        //        };

        //        TempData["OrderDetails"] = JsonConvert.SerializeObject(model);
        //    }
        //    else
        //    {
        //        var errorMessage = res != null ? string.Join(", ", res.Errors) : "An error occurred while processing the payment.";
        //        return Json(new { success = false, message = errorMessage });
        //    }

        //    return Json(new { success = true, message = "Payment processed successfully." });

        //}




        [HttpPost("save-payment")]
        public async Task<IActionResult> SavePayment([FromBody] PaymentDataRequest data)
        {
            if (data == null || string.IsNullOrEmpty(data.Payment?.TokenId))
            {
                return Json(new { success = false, message = "Invalid payment data" });
            }

            var response = await _productService.SavePaymentMaster(data);

            if (response != null && response.IsSuccess && response.Data is JsonElement element)
            {
                try
                {
                    // -------------- REMOVE SELECTED CART ITEMS START ---------------
                    // Read cart from session
                    var cart = Request.GetObject<List<ProductSessionModel>>("CartList") ?? new List<ProductSessionModel>();

                    // Extract purchased product IDs
                    var purchasedIds = data.Items.Select(i => i.ProductId).ToList();

                    // Remove only those purchased items from session cart
                    cart.RemoveAll(x => purchasedIds.Contains(x.Id));

                    // Save updated cart back to session
                    Response.SetObject("CartList", cart);
                    // -------------- REMOVE SELECTED CART ITEMS END ---------------

                    var model = new OrderDetailsViewModel
                    {
                        Product = data.Items.Select(p => new ProductSessionModel
                        {
                            Id = p.ProductId,
                            Quantity = int.TryParse(p.Quantity, out var qty) ? qty : 0,
                            Price = decimal.TryParse(p.Price, out var price) ? price : 0,
                            SalePrice = (decimal.TryParse(p.Price, out price) && decimal.TryParse(p.Discount, out var discount))
                                        ? price - discount : 0
                        }).ToList(),

                        ShippingDetails = data,

                        OrderId = element.TryGetProperty("orderId", out var orderIdProp) &&
                        int.TryParse(orderIdProp.GetString(), out var orderId) ? orderId : 0,

                        TotalAmount = decimal.TryParse(data.Amount, out var amt) ? amt : 0,

                        ExpectedDate = element.TryGetProperty("expectedDate", out var expProp) &&
                     DateTime.TryParse(expProp.GetString(), out var expDate) ? expDate : DateTime.Now,

                        PaymentDate = element.TryGetProperty("paymentDate", out var payProp) &&
                    DateTime.TryParse(payProp.GetString(), out var payDate) ? payDate : DateTime.Now
                    };

                    TempData["OrderDetails"] = JsonConvert.SerializeObject(model);

                    return Json(new { success = true }); // Redirect from JS side
                }
                catch (Exception ex)
                {
                    TempData["ToastError"] = "Payment saved, but error parsing order details.";
                    return Json(new { success = false, message = "Error processing order details" });
                }
            }

            TempData["ToastError"] = "Payment failed to save!";
            return Json(new { success = false, message = "Payment failed" });
        }

    }
}
