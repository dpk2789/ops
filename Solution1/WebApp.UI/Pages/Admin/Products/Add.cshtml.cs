using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.UI.Pages.Products
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }     

      
        public class InputModel
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
