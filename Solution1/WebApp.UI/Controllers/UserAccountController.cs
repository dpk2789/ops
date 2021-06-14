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
using WebApp.UI.Models;

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


        public void AddProductToCart(Guid id)
        {
            _sessionManager.AddProductToSession(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartPartialView()
        {
            using var client = new HttpClient();
            //HTTP get user info
            var cartListUri = new Uri("https://localhost:44347/api/Cart/GetCartItems/?userId=" + User.Identity.Name);

            var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken")?.Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

            var getUserInfo = await client.GetAsync(cartListUri);

            string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>(resultuerinfo);
            var CartList = data;
            return PartialView("_CartPartial", CartList);
        }
    }
}
