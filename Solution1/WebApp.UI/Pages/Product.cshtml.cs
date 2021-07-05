using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.UI.Helpers;

namespace WebApp.UI.Pages
{
    public class ProductModel : PageModel
    {
        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }            
            public IEnumerable<ProductImage> ProductImages { get; set; }
        }
        public class ProductImage
        {
            public Guid ProductId { get; set; }
            public string Name { get; set; }
            public long Width { get; set; }
            public string RelativePath { get; set; }
            public string GlobalPath { get; set; }
            public string Type { get; set; }
            public string Extention { get; set; }
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            using var client = new HttpClient();
            var updateProductsUri = new Uri(ApiUrls.Product.GetProduct + "?id=" + id);

            var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken").Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

            var postTask = await client.GetAsync(updateProductsUri);
            var result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var data = JsonConvert.DeserializeObject<InputModel>(result);
            Input = data;
          
            if (data == null)
                return RedirectToPage("Index");
            else
                return Page();
        }

    }
}
