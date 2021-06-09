using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.UI.Helpers;

namespace WebApp.UI.Pages.Products
{
    public class IndexModel : PageModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }

        [BindProperty]
        public ProductViewModel CartViewModel { get; set; }
        public class ProductViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
        }

        public async Task OnGet()
        {
            using (var client = new HttpClient())
            {               

                //HTTP get user info
                Uri getProducts = new Uri("https://localhost:44347/api/Products/GetProducts");

                var userAccessToken = User.Claims.Where(x => x.Type == "AcessToken").FirstOrDefault().Value;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);
                //HTTP get user info

                var getUserInfo = await client.GetAsync(getProducts);

                string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var data = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(resultuerinfo);
                Products = data;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var currentId = HttpContext.Session.GetString("id");
            HttpContext.Session.SetString("id", CartViewModel.Id.ToString());
            return Page();
        }
    }
}
