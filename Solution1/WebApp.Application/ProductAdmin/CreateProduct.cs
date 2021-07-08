using OnlineShop.Domain.Interface;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aow.Application.ProductAdmin
{
    [Service]
    public class CreateProduct
    {
        private IProductRepository _productManager;

        public CreateProduct(IProductRepository productManager)
        {
            _productManager = productManager;
        }
        public class CreateRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public List<ProductImageCreateRequest> ProductImages { get; set; }

        }
        public class ProductImageCreateRequest
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public long Length { get; set; }
            public string RelativePath { get; set; }
            public string GlobalPath { get; set; }
            public string Extention { get; set; }            
            public decimal Path { get; set; }
        }
        public class CreateResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
        public async Task<CreateResponse> Do(CreateRequest request)
        {
            Guid ProductId = Guid.NewGuid();
            var product = new OnlineShop.Domain.Models.Product
            {
                Id = ProductId,
                Name = request.Name,
                Value = request.Value,

                ProductImages = request.ProductImages.Select(x => new ProductImage
                {
                    Id= Guid.NewGuid(),
                    ProductId = ProductId,
                    Name = x.Name,
                    GlobalPath = x.GlobalPath,
                    RelativePath = x.RelativePath,
                    Length = x.Length,
                }).ToList()
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


    }
}
