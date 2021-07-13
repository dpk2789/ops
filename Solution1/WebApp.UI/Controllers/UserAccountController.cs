using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.UI.Helpers;
using WebApp.UI.Models;
using WebApp.UI.Models.cart;
using WebApp.UI.Pages;
using static WebApp.UI.Pages.Products.IndexModel;

namespace WebApp.UI.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly ISessionManager _sessionManager;

        public UserAccountController(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            foreach (var key in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Append(key, "", new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });
            }

            return RedirectToPage("/Index");
        }

        public void AddOneProductToCartSession([FromBody] CartProductRequest request)
        {
            _sessionManager.AddProductToSession(request);
        }

        [HttpPost]
        public void RemoveOneProductToCartSession([FromBody] CartProductRequest request)
        {
            _sessionManager.RemoveOneQuantityFromCartSession(request);
        }

        public void RemoveProductToCartSession(Guid id)
        {
            _sessionManager.RemoveProduct(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartPartialView()
        {
            if (User.Identity.IsAuthenticated)
            {
                using var client = new HttpClient();
                var cartListUri = new Uri(ApiUrls.Cart.GetCartItems + "/?userId=" + User.Identity.Name);
                var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken")?.Value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

                var getUserInfo = await client.GetAsync(cartListUri);

                string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var data = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>(resultuerinfo);
                // var CartList = data;
                CartModel cartModel = new CartModel(_sessionManager);
                cartModel.CartList = data;
                return PartialView("_CartPartial", cartModel);
            }
            else
            {
                var list = _sessionManager
                    .GetCart(x => new CartViewModel
                    {
                        Name = x.Name,
                        Value = x.Value,
                        RealValue = x.Value,
                        ProductId = x.Id,
                        Qty = x.Qty
                    });

                var CartListSession = list;
                return PartialView("_CartPartial", CartListSession);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAdminProductsPartialView()
        {
            using var client = new HttpClient();
            var getProductsUri = new Uri(ApiUrls.Product.GetProducts);

            var userAccessToken = User.Claims.Where(x => x.Type == "AcessToken").FirstOrDefault().Value;
            var userAccessRole = User.Claims.Where(x => x.Type == "role").FirstOrDefault().Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

            var getUserInfo = await client.GetAsync(getProductsUri);

            string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(resultuerinfo);
            return PartialView("_AdminProductsPartial", data);
        }

    }
}
