using OnlineShop.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aow.Application.Product
{
    [Service]
    public class GetProduct
    {
        private IProductRepository _productManager;

        public GetProduct(IProductRepository productManager)
        {
            _productManager = productManager;
        }
        public class GetProductViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IEnumerable<ProductImageResponse> ProductImages { get; set; }
        }
        public class ProductImageResponse
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

        public GetProductViewModel Do(string name)
        {
            var product = _productManager.GetProductByName(name, x => new GetProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Value = x.Value,
                Description = x.Description,
                ProductImages = x.ProductImages.Select(y => new ProductImageResponse
                {
                    ProductId = y.ProductId,
                    Id = y.Id,
                    Name = y.Name,
                    RelativePath = y.RelativePath,
                    Type = y.Type
                }),
            });

            return product;
        }

    }


}
