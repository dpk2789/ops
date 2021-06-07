using OnlineShop.Domain.Interface;
using OnlineShop.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Aow.Application.Cart
{
    [Service]
    public class AddToCart
    {
        private ICartRepository _cartRepository;

        public AddToCart(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public class AddToCartRequest
        {
            public Guid ProductId { get; set; }
            public int Qty { get; set; }
            public decimal Value { get; set; }
            public string UserName { get; set; }
        }

        public async Task<bool> Do(AddToCartRequest request)
        {
            // service responsibility          

            var cartProduct = new CartProduct()
            {
                ProductId = request.ProductId,
                Qty = request.Qty,
                Value = request.Value,
                UserId= request.UserName
            };

            var success = await _cartRepository.CreateCart(cartProduct) > 0;
            if (success)
            {
                return true;
            }

            return false;
        }
    }
}
