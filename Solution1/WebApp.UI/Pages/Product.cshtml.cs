using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.UI.Helpers;
using WebApp.UI.Models;

namespace WebApp.UI.Pages
{
    public class ProductModel : PageModel
    {
        private ISessionManager _sessionManager;
        public string ApiUrl { get; }
        public ProductModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            ApiUrl = ApiUrls.Rootlocal;
        }
        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public bool IsInCart { get; set; }
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

        public async Task<IActionResult> OnGet(string name)
        {
            using var client = new HttpClient();
            var updateProductsUri = new Uri(ApiUrls.Product.GetProductByName + "?name=" + name.Replace("-", " "));

            var postTask = await client.GetAsync(updateProductsUri);
            var result = postTask.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var data = JsonConvert.DeserializeObject<InputModel>(result);
            Input = data;

            var cartList = _sessionManager.GetCart(x => new InputModel
            {
                Name = x.Name,
                Value = x.Value,
                Id = x.ProductId,
            });

            foreach (var product in cartList)
            {
                if (cartList != null)
                {
                    var productViewModels = cartList.ToList();
                    if (productViewModels.Any(x => x.Id == Input.Id))
                    {
                        product.IsInCart = true;
                    }
                    else
                    {
                        product.IsInCart = false;
                    }
                }
            }
            if (data == null)
                return RedirectToPage("Index");
            else
                return Page();
        }

    }
}
