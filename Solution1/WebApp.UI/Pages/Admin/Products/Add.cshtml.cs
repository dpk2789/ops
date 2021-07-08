using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.UI.Helpers;
using WebApp.UI.Models;

namespace WebApp.UI.Pages.Admin.Products
{
    public class AddModel : PageModel
    {
        private IWebHostEnvironment _env;
        private string _dir;

        public AddModel(IWebHostEnvironment env)
        {
            _env = env;
            _dir = _env.ContentRootPath;
        }
        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IEnumerable<IFormFile> MainFiles { get; set; }
            public IFormFile BackImage { get; set; }
            public IFormFile FrontImage { get; set; }
            public IFormFile PackingImage { get; set; }
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
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            using var client = new HttpClient();
            var addProductsUri = new Uri(ApiUrls.Product.Create);
            var productImages = new List<ProductImage>();
            foreach (var file in Input.MainFiles)
            {
                var save_path = Path.Combine(_env.WebRootPath, file.FileName);
                using var fileStream = new FileStream(save_path, FileMode.Create, FileAccess.Write);
                file.CopyTo(fileStream);
                productImages.Add(new ProductImage
                {
                    Name = file.FileName,
                    Width = file.Length,
                    RelativePath = $"/{file.FileName}",
                    GlobalPath = save_path,
                    Type = "main"
                });
            }
            var json = JsonConvert.SerializeObject(new { Input.Name, Input.Value, Input.Description, productImages });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken")?.Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

            var postTask = await client.PostAsync(addProductsUri, content);
            //postTask.Wait();
            var result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<ApiResponse>(result);
            return RedirectToPage("Index");
        }
    }
}