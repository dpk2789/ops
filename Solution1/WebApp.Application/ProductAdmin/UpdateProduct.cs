using OnlineShop.Domain.Interface;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public class UpdateProductRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public List<UpdateProductImageRequest> ProductImages { get; set; }
        }
        public class UpdateProductImageRequest
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public long Length { get; set; }
            public string RelativePath { get; set; }
            public string GlobalPath { get; set; }
            public string Extention { get; set; }
            public decimal Path { get; set; }
        }
        public class UpdateProductResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
        public async Task<UpdateProductResponse> Do(UpdateProductRequest request)
        {
            var product = _productManager.GetProductById(request.Id, x => x);
            product.Name = request.Name;
            product.Value = request.Value;
            product.Description = request.Description;

            var mainImage = product.ProductImages.FirstOrDefault();

            foreach (var image in request.ProductImages)
            {
                if (product.ProductImages.Any(x => x.Type == image.Type))
                {
                    var retriveImage = product.ProductImages.FirstOrDefault(x => x.Type == image.Type);
                    retriveImage.Name = image.Name;
                    retriveImage.Type = image.Type;
                    retriveImage.GlobalPath = image.GlobalPath;
                    retriveImage.RelativePath = image.RelativePath;
                    await _productManager.UpdateProductImage(retriveImage);
                }
                else
                {
                    ProductImage productImage = new ProductImage();
                    productImage.Id = Guid.NewGuid();
                    productImage.ProductId = product.Id;
                    productImage.Name = image.Name;
                    productImage.GlobalPath = image.GlobalPath;
                    productImage.RelativePath = image.RelativePath;
                    productImage.Type = image.Type;
                    await _productManager.AddProductImage(productImage);
                }
             
            }

            if (await _productManager.UpdateProduct(product) <= 0)
            {
                throw new Exception("Failed to create product");
            }

            return new UpdateProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value
            };
        }


    }
}
