using OnlineShop.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace Aow.Application.ProductAdmin
{
    [Service]
    public class UpdateProduct
    {
        private IProductRepository _productManager;

        public UpdateProduct(IProductRepository productManager)
        {
            _productManager = productManager;
        }

        public async Task<Response> Do(Request request)
        {
            var product = _productManager.GetProductById(request.Id, x => x);

            product.Name = request.Name;           
            product.Value = request.Value;

            await _productManager.UpdateProduct(product);

            return new Response
            {
                Id = product.Id,
                Name = product.Name,                
                Value = product.Value
            };
        }

        public class Request
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
