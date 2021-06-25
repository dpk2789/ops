using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.UI.Models;

namespace WebApp.UI.Pages.CheckOut
{
    public class CustomerImformationModel : PageModel
    {
        private IWebHostEnvironment _env;
        private readonly ISessionManager _sessionManager;

        public CustomerImformationModel(IWebHostEnvironment env, ISessionManager sessionManager)
        {
            _env = env;
            _sessionManager = sessionManager;
        }

        [BindProperty]
        public CustomerInformation customerInformation { get; set; }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {

                return RedirectToPage("/Checkout/Payment");
            }
            var information = _sessionManager.GetCustomerInformation();

            if (information == null)
            {
                //customerInformation.FirstName = information.FirstName;
                return Page();
            }
            else
            {
                return RedirectToPage("/Checkout/Payment");
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _sessionManager.AddCustomerInformation(customerInformation);

            return RedirectToPage("/Checkout/Payment");
        }
    }
}
