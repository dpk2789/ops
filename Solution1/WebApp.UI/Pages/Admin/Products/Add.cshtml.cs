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
            public IEnumerable<IFormFile> Files { get; set; }
        }
        private List<ProductImage> ProductImages;
        public async Task<IActionResult> OnPost()
        {          
            foreach (var file in Input.Files)
            {
                var save_path = Path.Combine(_env.WebRootPath, file.FileName);
                using var fileStream = new FileStream(save_path, FileMode.Create, FileAccess.Write);
                file.CopyTo(fileStream);
                ProductImage image = new ProductImage();
                image.Id = Guid.NewGuid();
                image.Width = (int)file.Length;
                image.RelativePath = $"/{file.FileName}";
                image.GlobalPath = save_path;
                ProductImages.Add(image);
                //ProductImages.Add(new ProductImage
                //{
                //    Id = Guid.NewGuid(),
                //    Width = (int)file.Length,
                //    RelativePath = $"/{file.FileName}",
                //    GlobalPath = save_path
                //});
            }
            
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