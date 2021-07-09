using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IWebHostEnvironment _env;
        private string _dir;

        public UpdateModel(IWebHostEnvironment env)
        {
            _env = env;
            _dir = _env.ContentRootPath;
        }
        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IFormFile File { get; set; }
            public IFormFile BackImage { get; set; }
            public IFormFile FrontImage { get; set; }
            public IFormFile PackingImage { get; set; }
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
            var productImages = new List<ProductImage>();
            if (Input.File != null)
            {
                var save_path = Path.Combine(_env.WebRootPath, Input.File.FileName);
                using var fileStream = new FileStream(save_path, FileMode.Create, FileAccess.Write);
                Input.File.CopyTo(fileStream);
                productImages.Add(new ProductImage
                {
                    Name = Input.File.FileName,
                    Width = Input.File.Length,
                    RelativePath = $"/{Input.File.FileName}",
                    GlobalPath = save_path,
                    Type = "main"
                });
            }
            if (Input.FrontImage != null)
            {
                var save_path = Path.Combine(_env.WebRootPath, Input.FrontImage.FileName);
                using var fileStream = new FileStream(save_path, FileMode.Create, FileAccess.Write);
                Input.FrontImage.CopyTo(fileStream);
                productImages.Add(new ProductImage
                {
                    Name = Input.FrontImage.FileName,
                    Width = Input.FrontImage.Length,
                    RelativePath = $"/{Input.FrontImage.FileName}",
                    GlobalPath = save_path,
                    Type = "front"
                });
            }           
            if (Input.BackImage != null)
            {
                var save_path = Path.Combine(_env.WebRootPath, Input.BackImage.FileName);
                using var fileStream = new FileStream(save_path, FileMode.Create, FileAccess.Write);
                Input.BackImage.CopyTo(fileStream);
                productImages.Add(new ProductImage
                {
                    Name = Input.BackImage.FileName,
                    Width = Input.BackImage.Length,
                    RelativePath = $"/{Input.BackImage.FileName}",
                    GlobalPath = save_path,
                    Type = "back"
                });
            }
            if (Input.PackingImage != null)
            {
                var save_path = Path.Combine(_env.WebRootPath, Input.PackingImage.FileName);
                using var fileStream = new FileStream(save_path, FileMode.Create, FileAccess.Write);
                Input.PackingImage.CopyTo(fileStream);
                productImages.Add(new ProductImage
                {
                    Name = Input.PackingImage.FileName,
                    Width = Input.PackingImage.Length,
                    RelativePath = $"/{Input.PackingImage.FileName}",
                    GlobalPath = save_path,
                    Type = "packing"
                });
            }
            using var client = new HttpClient();
            var addProductsUri = new Uri(ApiUrls.Product.Update);
            
            var json = JsonConvert.SerializeObject(new { Input.Id, Input.Name, Input.Value, Input.Description , productImages });
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
