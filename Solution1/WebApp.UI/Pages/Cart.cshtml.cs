using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using WebApp.UI.Models;

namespace WebApp.UI.Pages
{
    public class CartModel : PageModel
    {
        private ISessionManager _sessionManager;

        public CartModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public IEnumerable<CartResponse> CartList { get; set; }
        public class CartResponse
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public decimal RealValue { get; set; }
            public int Qty { get; set; }
            public Guid ProductId { get; set; }
        }

        public IActionResult OnGet()
        {
            var list = _sessionManager
                .GetCart(x => new CartResponse
                {
                    Name = x.ProductName,
                    Value = x.Value.ToString(),
                    RealValue = x.Value,
                    ProductId = x.ProductId,
                    Qty = x.Qty
                });

            CartList = list;
            return Page();
        }
    }
}
