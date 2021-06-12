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

        public CartModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public IEnumerable<CartViewModel> CartList { get; set; }

        public async Task<IActionResult> OnGet()
        {
            //var list = _sessionManager
            //    .GetCart(x => new CartResponse
            //    {
            //        Name = x.ProductName,
            //        Value = x.Value.ToString(),
            //        RealValue = x.Value,
            //        ProductId = x.ProductId,
            //        Qty = x.Qty
            //    });

            //CartList = list;

            using (var client = new HttpClient())
            {
                //HTTP get user info
                Uri cartListUri = new Uri(ApiUrls.Cart.GetCartItems + "/?userId=" + User.Identity.Name);

                var userAccessToken = User.Claims.Where(x => x.Type == "AcessToken").FirstOrDefault().Value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

                var getUserInfo = await client.GetAsync(cartListUri);

                string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var data = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>(resultuerinfo);
                CartList = data;
                return Page();
            }
        }
    }
}
