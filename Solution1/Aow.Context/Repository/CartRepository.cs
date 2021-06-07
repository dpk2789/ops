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

        public Task<int> CreateCart(CartProduct stock)
        {
            _ctx.CartProducts.Add(stock);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteCart(Guid id)
        {
            var stock = _ctx.CartProducts.FirstOrDefault(x => x.Id == id);
            _ctx.CartProducts.Remove(stock);
            return _ctx.SaveChangesAsync();
        }

        public Task<int> UpdateCart(List<CartProduct> stockList)
        {
            _ctx.CartProducts.UpdateRange(stockList);
            return _ctx.SaveChangesAsync();
        }
    }
}
