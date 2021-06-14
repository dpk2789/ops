using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.UI.Helpers;
using WebApp.UI.Models;

namespace WebApp.UI.Pages.Admin.Products
{
    public class AddModel : PageModel
    {
        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            using var client = new HttpClient();
            var addProductsUri = new Uri(ApiUrls.Product.Create);
            Input.Description = "sample";
            var json = JsonConvert.SerializeObject(new { Input.Name, Input.Value, Input.Description });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken").Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

            var postTask = await client.PostAsync(addProductsUri, content);
            //postTask.Wait();
            var result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<ApiResponse>(result);
            return RedirectToPage("Index");
        }
    }
}