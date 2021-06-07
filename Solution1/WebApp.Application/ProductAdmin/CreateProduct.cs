using OnlineShop.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace Aow.Application.ProductAdmin
{
    public class CreateProduct
    {
        private IProductRepository _productManager;

        public CreateProduct(IProductRepository productManager)
        {
            _productManager = productManager;
        }

        public async Task<CreateResponse> Do(CreateRequest request)
        {
            var product = new OnlineShop.Domain.Models.Product
            {
                Name = request.Name,               
                Value = request.Value
            };

            if (await _productManager.CreateProduct(product) <= 0)
            {
                throw new Exception("Failed to create product");
            }

            return new CreateResponse
            {
                Id = product.Id,
                Name = product.Name,               
                Value = product.Value
            };
        }

        public class CreateRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public class CreateResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
