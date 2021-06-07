using OnlineShop.Domain.Interface;
using System;
using System.Collections.Generic;

namespace Aow.Application.ProductAdmin
{
    [Service]
    public class GetProducts
    {
        private IProductRepository _productRepository;

        public GetProducts(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _productRepository.GetProductsWithStock(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Value = x.Value,
            });

        public class ProductViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
        }
    }
}
