using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
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
        public string ApiUrl { get; }
        public IndexModel()
        {
            ApiUrl = ApiUrls.Rootlocal;
        }       
        public IEnumerable<ProductViewModel> Products { get; set; }

        [BindProperty]
        public ProductViewModel AdminProductViewModel { get; set; }
       
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
                var getProductsUri = new Uri(ApiUrls.Product.GetProducts);

                var userAccessToken = User.Claims.Where(x => x.Type == "AcessToken").FirstOrDefault().Value;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);              

                var getUserInfo = await client.GetAsync(getProductsUri);

                string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var data = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(resultuerinfo);
                Products = data;
            }
        }

        //public async Task<IActionResult> OnPost()
        //{
        //    var currentId = HttpContext.Session.GetString("id");
        //    HttpContext.Session.SetString("id", CartViewModel.Id.ToString());
        //    return Page();
        //}
    }
}
