using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interface
{
    public interface IProductRepository
    {
        Task<int> CreateProduct(Product product);
        Task<int> DeleteProduct(Guid id);
        Task<int> UpdateProduct(Product product);
        Task<int> AddProductImage(ProductImage image);
        Task<int> UpdateProductImage(ProductImage image);
        TResult GetProductById<TResult>(Guid id, Func<Product, TResult> selector);
        TResult GetProductByName<TResult>(string name, Func<Product, TResult> selector);
        IEnumerable<TResult> GetProductsWithStock<TResult>(Func<Product, TResult> selector);
    }
}
