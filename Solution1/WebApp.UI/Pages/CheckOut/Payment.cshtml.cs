using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using WebApp.UI.Models;

namespace WebApp.UI.Pages.CheckOut
{
    public class PaymentModel : PageModel
    {
        private readonly ISessionManager _sessionManager;
        public string RazorPayKey { get; }
        public PaymentModel(ISessionManager sessionManager, IConfiguration config)
        {
            _sessionManager = sessionManager;
            RazorPayKey = config["RazorPay:Key"].ToString();
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Page();
            }
            else
            {
                var information = _sessionManager.GetCustomerInformation();

                if (information == null)
                {
                    return RedirectToPage("/Checkout/CustomerInformation");
                }
                return Page();
            }           
        }
        public IActionResult OnPost()
        {
           

            return Page();
        }
    }
}
