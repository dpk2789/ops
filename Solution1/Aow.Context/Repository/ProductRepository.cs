﻿

using OnlineShop.Domain.Interface;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aow.Context.Repository
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext _ctx;

        public ProductRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> CreateProduct(Product product)
        {
            _ctx.Products.Add(product);
            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteProduct(Guid id)
        {
            var product = _ctx.Products.FirstOrDefault(x => x.Id == id);
            _ctx.Products.Remove(product);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateProduct(Product product)
        {
            _ctx.Products.Update(product);
            return _ctx.SaveChangesAsync();
        }

        public TResult GetProductById<TResult>(Guid id, Func<Product, TResult> selector)
        {
            return _ctx.Products
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();
        }

        public TResult GetProductByName<TResult>(
            string name,
            Func<Product, TResult> selector)
        {
            return _ctx.Products              
                .Where(x => x.Name == name)
                .Select(selector)
                .FirstOrDefault();
        }

        public IEnumerable<TResult> GetProductsWithStock<TResult>(
            Func<Product, TResult> selector)
        {
            return _ctx.Products                
                .Select(selector)
                .ToList();
        }
    }
}
