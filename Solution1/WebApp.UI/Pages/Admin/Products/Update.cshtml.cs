using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApp.UI.Helpers;

namespace WebApp.UI.Pages.Products
{
    public class UpdateModel : PageModel
    {
        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
        public async Task<IActionResult> OnGet(Guid Id)
        {
            using var client = new HttpClient();

            var updateProductsUri = new Uri(ApiUrls.Product.GetProduct + "?id=" + Id);

            var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken").Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

            var postTask = await client.GetAsync(updateProductsUri);
            var result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var data = JsonConvert.DeserializeObject<InputModel>(result);
            Input = data;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            using var client = new HttpClient();
            var addProductsUri = new Uri(ApiUrls.Product.Update);
            Input.Description = "sample";
            var json = JsonConvert.SerializeObject(new { Input.Id, Input.Name, Input.Value, Input.Description });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken").Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

            var postTask = await client.PutAsync(addProductsUri, content);
            //postTask.Wait();
            var result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
             var data = JsonConvert.DeserializeObject<InputModel>(result);
            Input = data;
            return RedirectToPage("Index");
        }
    }
}
