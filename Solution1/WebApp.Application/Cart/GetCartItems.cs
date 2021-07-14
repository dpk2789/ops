using OnlineShop.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

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
            public IEnumerable<CartProductImageResponse> ProductImages { get; set; }
        }
        public class CartProductImageResponse
        {
            public Guid Id { get; set; }
            public Guid ProductId { get; set; }
            public string Name { get; set; }
            public long Width { get; set; }
            public string RelativePath { get; set; }
            public string GlobalPath { get; set; }
            public string Type { get; set; }
            public string Extention { get; set; }
        }

        public IEnumerable<Response> Do(string userId)
        {
            return _cartRepository.GetCartProducts(userId, x => new Response
            {
                Name = x.Product.Name,
                Value = x.Product.Value,
                RealValue = x.Value,
                ProductId = x.ProductId,
                Qty = x.Qty,

                ProductImages = x.Product.ProductImages.Select(y => new CartProductImageResponse
                {
                    ProductId = y.ProductId,
                    Id = y.Id,
                    Name = y.Name,
                    RelativePath = y.RelativePath,
                    Type = y.Type
                }),
            });
        }
    }
}
