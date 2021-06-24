using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.UI.Models;

namespace WebApp.UI.Pages.CheckOut
{
    public class PaymentModel : PageModel
    {
        private readonly ISessionManager _sessionManager;
        public string PublicKey { get; }
        public PaymentModel(ISessionManager sessionManager, IConfiguration config)
        {
            _sessionManager = sessionManager;
            PublicKey = config["Stripe:PublicKey"].ToString();
        }
        public IActionResult OnGet()
        {
            var information = _sessionManager.GetCustomerInformation();

            if (information == null)
            {
                return RedirectToPage("/Checkout/CustomerInformation");
            }
           
            return Page();
        }
        public async Task<IActionResult> OnPost(string stripeEmail, string stripeToken)
        {
            var token = stripeToken; // Using ASP.NET MVC

            var options = new ChargeCreateOptions
            {
                Amount = 999,
                Currency = "INR",
                Description = "Example charge",
                Source = token,
            };

            var service = new ChargeService();
            var charge = service.Create(options);
            return Page();

        }
    }
}
