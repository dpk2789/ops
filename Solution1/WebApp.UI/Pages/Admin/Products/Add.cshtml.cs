using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.UI.Pages.Admin.Products
{
    public abstract class AddModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public abstract class InputModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            return Page();
        }
    }
}
