using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.UI.Models;

namespace WebApp.UI.Helpers
{
    public class SessionManager : ISessionManager
    {
        private readonly ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IEnumerable<TResult> GetCart<TResult>(Func<CartProductViewModel, TResult> selector)
        {
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
                return new List<TResult>();

            var cartList = JsonConvert.DeserializeObject<IEnumerable<CartProductViewModel>>(stringObject);

            return cartList.Select(selector);
        }

        public void AddProductToSession(Guid Id)
        {
            //var currentId = HttpContext.Session.GetString("cart");
            //HttpContext.Session.SetString("id", Id.ToString());
            CartProductViewModel productViewModel = new CartProductViewModel();
            productViewModel.ProductId = Id;
            var cartList = new List<CartProductViewModel>();
            var stringObject = _session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProductViewModel>>(stringObject);
            }
            cartList.Add(productViewModel);
            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }

        public void ClearCart()
        {
            _session.Remove("cart");
        }

        public void RemoveProduct(Guid productId, int qty)
        {
            var cartList = new List<CartProductViewModel>();
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject)) return;

            cartList = JsonConvert.DeserializeObject<List<CartProductViewModel>>(stringObject);

            if (!cartList.Any(x => x.ProductId == productId)) return;

            var product = cartList.First(x => x.ProductId == productId);
            product.Qty -= qty;

            if (product.Qty <= 0)
            {
                cartList.Remove(product);
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }
    }
}
