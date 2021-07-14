using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.UI.Helpers;
using WebApp.UI.Models;

namespace WebApp.UI.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            ILogger<RegisterModel> logger, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; private set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public IActionResult OnGetAsync(string returnUrl)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
           
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {            
            if (!ModelState.IsValid) return Page();
            using var client = new HttpClient();
            var u = new Uri(ApiUrls.Identity.Register);

            var json = JsonConvert.SerializeObject(new { Input.Email, Input.Password, Input.ConfirmPassword });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //HTTP POST
            var postTask = await client.PostAsync(u, content);
            //postTask.Wait();
            var result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<AuthRegiterResponse>(result);

            //var callbackUrl = Url.ActionLink(
            //    "Account/ConfirmEmail",
            //    values: new { userId = data.UserId, code = data.Msg, returnUrl },
            //    protocol: Request.Scheme);

            var url = "https://localhost:44324/Account/ConfirmEmail?userId=" + data.UserId + "&code=" + data.Msg;
            var link = $"<a href='{url}'>Click here</a>";

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a>.");

            if (data.Success)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
            }

            foreach (var error in data.ErrorMessages)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return Page();
            // If we got this far, something failed, redisplay form
        }
    }
}
