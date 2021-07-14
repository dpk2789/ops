using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Interface;
using OnlineShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aow.Context.Repository
{
    public class CartRepository : ICartRepository
    {
        private ApplicationDbContext _ctx;

        public CartRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<TResult> GetCartProducts<TResult>(string userId, Func<CartProduct, TResult> selector)
        {
            return _ctx.CartProducts
                .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                .Where(x => x.UserId == userId)
                .Select(selector)
                .ToList();
        }

        public Task<int> CreateCart(CartProduct stock)
        {
            _ctx.CartProducts.Add(stock);
            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteCart(Guid productId, string userId)
        {
            var cartProduct = _ctx.CartProducts
               .FirstOrDefault(x => x.ProductId == productId
                           && x.UserId == userId);
            _ctx.CartProducts.Remove(cartProduct);
            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateCart(List<CartProduct> stockList)
        {
            _ctx.CartProducts.UpdateRange(stockList);
            return _ctx.SaveChangesAsync();
        }


        public Task<int> AddOneToCart(Guid productId, int qty, string userId)
        {
            var cartProduct = _ctx.CartProducts
                .FirstOrDefault(x => x.ProductId == productId
                            && x.UserId == userId);

            if (cartProduct != null)
            {
                cartProduct.Qty += qty;
                _ctx.CartProducts.UpdateRange(cartProduct);
            }
            else
            {
                _ctx.CartProducts.Add(new CartProduct
                {
                    ProductId = productId,
                    UserId = userId,
                    Qty = qty,
                });
            }
            return _ctx.SaveChangesAsync();
        }
    }
}
