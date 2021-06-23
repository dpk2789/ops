using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.UI.Models;
using WebApp.UI.Models.cart;

namespace WebApp.UI.Helpers
{
    public class SessionManager : ISessionManager
    {
        private readonly ISession _session;
        private const string KeyCart = "cart";
        private const string KeyCustomerInfo = "customer-info";
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }
        public void AddCustomerInformation(CustomerInformation customer)
        {
            var stringObject = JsonConvert.SerializeObject(customer);

            _session.SetString(KeyCustomerInfo, stringObject);
        }
        public CustomerInformation GetCustomerInformation()
        {
            var stringObject = _session.GetString(KeyCustomerInfo);

            if (string.IsNullOrEmpty(stringObject))
                return null;

            var customerInformation = JsonConvert.DeserializeObject<CustomerInformation>(stringObject);

            return customerInformation;
        }
        public IEnumerable<TResult> GetCart<TResult>(Func<CartProductViewModel, TResult> selector)
        {
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
                return new List<TResult>();

            var cartList = JsonConvert.DeserializeObject<IEnumerable<CartProductViewModel>>(stringObject);

            return cartList.Select(selector);
        }

        public void AddProductToSession(CartProductRequest request)
        {
            //var currentId = HttpContext.Session.GetString("cart");
            //HttpContext.Session.SetString("id", Id.ToString());           

            var cartList = new List<CartProductViewModel>();
            var stringObject = _session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProductViewModel>>(stringObject);
            }

            if (cartList.Any(x => x.Id == request.ProductId))
            {
                var findproductViewModel = cartList.Find(x => x.Id == request.ProductId);
                findproductViewModel.Qty = request.Qty + 1;
            }
            else
            {
                cartList.Add(new CartProductViewModel
                {
                    Id = request.ProductId,
                    Name = request.Name,
                    ProductId = request.ProductId,
                    Qty = request.Qty
                });
            }

            stringObject = JsonConvert.SerializeObject(cartList);
            _session.SetString("cart", stringObject);
        }

        public void ClearCart()
        {
            _session.Remove("cart");
        }

        public void RemoveProduct(Guid productId)
        {
            var cartList = new List<CartProductViewModel>();
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject)) return;

            cartList = JsonConvert.DeserializeObject<List<CartProductViewModel>>(stringObject);

            if (!cartList.Any(x => x.Id == productId)) return;

            var product = cartList.First(x => x.Id == productId);
            cartList.Remove(product);

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }


        public void RemoveOneQuantityFromCartSession(CartProductRequest productViewModel)
        {
            var cartList = new List<CartProductViewModel>();
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject)) return;

            cartList = JsonConvert.DeserializeObject<List<CartProductViewModel>>(stringObject);

            if (!cartList.Any(x => x.Id == productViewModel.ProductId)) return;

            var product = cartList.First(x => x.Id == productViewModel.ProductId);
            product.Qty = product.Qty - 1;

            if (product.Qty <= 0)
            {
                cartList.Remove(product);
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }
    }
}
