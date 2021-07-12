using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.UI.Helpers;
using WebApp.UI.Models;

namespace WebApp.UI.Pages
{
    public class IndexModel : PageModel
    {
        private ISessionManager _sessionManager;
        public string ApiUrl { get; }
        public IndexModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;           
            ApiUrl = ApiUrls.Rootlocal;
        }

        private readonly ILogger<IndexModel> _logger;
        public IEnumerable<ProductViewModel> Products { get; set; }

        [BindProperty]
        public ProductViewModel CartViewModel { get; set; }
        public class ProductViewModel
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

        public async Task OnGet()
        {

            using var client = new HttpClient();
            var getProductsUri = new Uri(ApiUrls.Product.GetProducts);
            var getUserInfo = await client.GetAsync(getProductsUri);

            string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(resultuerinfo);
            Products = data;

            var cartList = _sessionManager.GetCart(x => new ProductViewModel
            {
                Name = x.Name,
                Value = x.Value,
                Id = x.ProductId,
            });

            foreach (var product in Products)
            {
                if (cartList != null)
                {
                    var productViewModels = cartList.ToList();
                    if (productViewModels.Any(x => x.Id == product.Id))
                    {
                        product.IsInCart = true;
                    }
                    else
                    {
                        product.IsInCart = false;
                    }
                }
            }

        }
    }
}
