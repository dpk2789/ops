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

            var razorpayClient = new RazorpayClient("rzp_test_PhqnP6sane3Ovm", "tWL6ajNZ58SXI7Q9O1ayoy5A");

            var options = new Dictionary<string, object>
        {
            { "amount", 200 },
            { "currency", "INR" },
            { "receipt", "recipt_1" },
            // auto capture payments rather than manual capture
            // razor pay recommended option
            { "payment_capture", true }
        };

            var order = razorpayClient.Order.Create(options);
            var orderId = order["id"].ToString();
            var orderJson = order.Attributes.ToString();
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
