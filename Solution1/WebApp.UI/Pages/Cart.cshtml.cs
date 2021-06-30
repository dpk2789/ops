using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.UI.Helpers;
using WebApp.UI.Models;

namespace WebApp.UI.Pages
{
    public class CartModel : PageModel
    {
        private ISessionManager _sessionManager;
        public string ApiUrl { get; }
        public CartModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            ApiUrl = ApiUrls.Rootlocal;
        }

        public IEnumerable<CartViewModel> CartList { get; set; }

        public decimal GetTotalCharge() => CartList.Sum(x => x.Value * x.Qty);

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                using var client = new HttpClient();
                //HTTP get user info
                Uri cartListUri = new Uri(ApiUrls.Cart.GetCartItems + "/?userId=" + User.Identity.Name);

                var userAccessToken = User.Claims.Where(x => x.Type == "AcessToken").FirstOrDefault().Value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

                var getUserInfo = await client.GetAsync(cartListUri);

                var resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var list = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>(resultuerinfo);
                CartList = list;
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
                CartList = list;

            }
            return Page();

        }
    }
}
