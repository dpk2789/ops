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

        public IndexModel(ISessionManager sessionManager, ILogger<IndexModel> logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
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
            using (var client = new HttpClient())
            {
                var cartList = _sessionManager
              .GetCart(x => new ProductViewModel
              {
                  Name = x.ProductName,
                  Value = x.Value,
                  Id = x.ProductId,
              });

                Uri u = new Uri(IdentityUrls.Identity.Login);

                //HTTP get user info
                Uri userinfo = new Uri("https://localhost:44347/api/Products/GetProducts");

                //HTTP get user info

                var getUserInfo = await client.GetAsync(userinfo);

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
}
