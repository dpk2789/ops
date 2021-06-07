using OnlineShop.Domain.Interface;
using System;

namespace Aow.Application.ProductAdmin
{
    public class GetProduct
    {
        private IProductRepository _productManager;

        public GetProduct(IProductRepository productManager)
        {
            _productManager = productManager;
        }

        public ProductViewModel Do(Guid id) =>
            _productManager.GetProductById(id, x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,               
                Value = x.Value,
            });
    }

    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
    }
}
