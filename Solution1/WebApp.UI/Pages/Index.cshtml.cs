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
        public IndexModel(ISessionManager sessionManager, ILogger<IndexModel> logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
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
            public decimal Value { get; set; }
            public bool IsInCart { get; set; }
        }

        public async Task OnGet()
        {
            using var client = new HttpClient();
            var cartList = _sessionManager.GetCart(x => new ProductViewModel
            {
                Name = x.Name,
                Value = x.Value,
                Id = x.ProductId,
            });

            Uri getProductsUri = new Uri(ApiUrls.Product.GetProducts);
            var getUserInfo = await client.GetAsync(getProductsUri);

            string resultuerinfo = getUserInfo.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(resultuerinfo);
            Products = data;

            foreach (var product in Products)
            {
                if (cartList.Any(x => x.Id == product.Id))
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
