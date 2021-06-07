using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.UI.Models;

namespace WebApp.UI.Controllers
{
    public class UserAccountController : Controller
    {
        private ISessionManager _sessionManager;

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


        public void AddProductToCart(Guid Id)
        {
            _sessionManager.AddProductToSession(Id);
        }
    }
}
