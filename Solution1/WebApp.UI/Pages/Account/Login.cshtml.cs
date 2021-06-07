using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using WebApp.UI.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.UI.Models;
using System.Net.Http.Headers;
using System.Text;

namespace WebApp.RazorPages.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public IActionResult OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");


            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    Uri u = new Uri(IdentityUrls.Identity.Login);

                    //var content = new FormUrlEncodedContent(new[]
                    //                {                                     
                    //                     new KeyValuePair<string, string>("email",Input.Email),
                    //                    new KeyValuePair<string, string>("password", Input.Password)
                    //                  });


                    var json = JsonConvert.SerializeObject(new { Input.Email, Input.Password });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //HTTP POST
                    var postTask = await client.PostAsync(u, content);
                    //postTask.Wait();
                    string result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var token = JsonConvert.DeserializeObject<Token>(result);

                    // client.SetBearerToken(token.access_token);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
                    //HTTP get user info
                    Uri userinfo = new Uri("https://localhost:44347/weatherforecast");
                    var getUserInfo = await client.GetAsync(userinfo);


                    string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //  var data = JsonConvert.DeserializeObject<UserInfo>(resultuerinfo);


                    AuthenticationProperties authProperties = new AuthenticationProperties();

                    authProperties.AllowRefresh = true;
                    authProperties.IsPersistent = true;
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddHours((token.expires_in));

                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, Input.Email),
                    new Claim("UserRoleClaim", "admin"),
                    new Claim("AcessToken", string.Format("{0}", token.token)),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                    if (postTask.IsSuccessStatusCode)
                    {
                        // sessionStorage.setItem("accessToken", token.token);
                        return RedirectToPage("/Index");
                    }

                    ModelState.AddModelError(string.Empty, result);
                    return Page();

                }

            }
            return Page();
        }
    }
}


