using OnlineShop.Domain.Interface;
using System;
using System.Collections.Generic;

namespace Aow.Application.Cart
{
    [Service]
    public class GetCartItems
    {
        private ICartRepository _cartRepository;

        public GetCartItems(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public class Response
        {
            public string Name { get; set; }
            public decimal Value { get; set; }
            public decimal Amount { get; set; }
            public decimal RealValue { get; set; }
            public int Qty { get; set; }
            public Guid ProductId { get; set; }
        }

        public IEnumerable<Response> Do(string userId)
        {
            return _cartRepository
                .GetCartProducts(userId, x => new Response
                {
                    Name = x.Product.Name,
                    Value = x.Product.Value,                   
                    RealValue = x.Value,
                    ProductId = x.ProductId,
                    Qty = x.Qty
                });
        }
    }
}
