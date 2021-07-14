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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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

        public IActionResult OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            return Page();
        }

        public static ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = "abc123456789";
            validationParameters.ValidIssuer = "abc123456789";
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbbb"));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                using var client = new HttpClient();
                Uri u = new Uri(ApiUrls.Identity.Login);

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
                var securitytoken = JsonConvert.DeserializeObject<Token>(result);


                var jwtToken = new JwtSecurityToken(securitytoken.token);
                var role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role");

                //var test = ValidateToken(securitytoken.token);
                //var rolesClaim = test.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                //var rolesClaim = test.Claims.Where(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).FirstOrDefault();
                var claims = new List<Claim>();
                if (role == null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, Input.Email));
                    claims.Add(new Claim("UserRoleClaim", "customer"));
                    claims.Add(new Claim("AcessToken", string.Format("{0}", securitytoken.token)));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Name, Input.Email));
                    claims.Add(new Claim("UserRoleClaim", role.Value));
                    claims.Add(new Claim("AcessToken", string.Format("{0}", securitytoken.token)));
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", securitytoken.token);
                //HTTP get user info
                //Uri userinfo = new Uri("http://api.robustpackagingeshop.com/weatherforecast");
                //var getUserInfo = await client.GetAsync(userinfo);
                //string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                //  var data = JsonConvert.DeserializeObject<UserInfo>(resultuerinfo);


                AuthenticationProperties authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours((securitytoken.expires_in))
                };



                var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
                if (postTask.IsSuccessStatusCode)
                {
                    // sessionStorage.setItem("accessToken", token.token);
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, result);
                return Page();

            }
            return Page();
        }
    }
}


